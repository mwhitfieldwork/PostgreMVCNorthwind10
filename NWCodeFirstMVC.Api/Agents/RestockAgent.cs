using System.Text.Json;
using Anthropic;
using Anthropic.Helpers.Beta;
using Anthropic.Models.Beta.Messages;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Infrastructure;
using NWCodeFirstMVC.Domain.Contracts;
using System.Net;
using NWCodeFirstMVC.Domain.Dto;
namespace NWCodeFirstMVC.Api.Agents;

public class RestockAgent
{
    private readonly PgNwContext _db;
    private readonly AnthropicClient _client;
    private readonly IEmailService _email;
    private readonly ICsvService _csv;
    private readonly string? _reportEmail;
    private readonly SemaphoreSlim _dbLock = new(1, 1);

    // Lets only one tool use the database at a time.
    private async Task<string> OneAtATime(Func<Task<string>> work, CancellationToken ct)
    {
        await _dbLock.WaitAsync(ct);
        try
        {
            return await work();
        }
        finally
        {
            _dbLock.Release();
        }
    }
    public RestockAgent(PgNwContext db, IConfiguration config, IEmailService email, ICsvService csv)
    {
        _db = db;
        _client = new AnthropicClient { ApiKey = config["Anthropic:ApiKey"] };
        _email = email;
        _csv = csv;
        _reportEmail = config["Agent:ReportEmail"];
    }

    public async Task<string> AskAsync(string question)
    {

        // The tool: a name + description Claude reads, and the C# code that runs.
        var lowStockTool = new BetaRunnableTool
        {
            Name = "get_low_stock_products",
            Definition = new BetaTool
            {
                Name = "get_low_stock_products",
                Description = "Lists active products whose units in stock plus units on order " +
                              "are at or below their reorder level.",
                InputSchema = new InputSchema { Properties = new Dictionary<string, JsonElement>() },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                var items = await _db.Products
                    .Where(p => p.Discontinued == 0 && !p.IsDeleted
                        && (p.UnitsInStock ?? 0) + (p.UnitsOnOrder ?? 0) <= (p.ReorderLevel ?? 0))
                    .Select(p => new { p.ProductName, p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel })
                    .ToListAsync(ct);

                return JsonSerializer.Serialize(items);
            }, ct),
        };

        var priceRankTool = new BetaRunnableTool
        {
            Name = "get_products_by_price",
            Definition = new BetaTool
            {
                Name = "get_products_by_price",
                Description = "Lists active products ranked by unit price. Use 'most' for the most " +
                      "expensive products or 'least' for the least expensive.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["order"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "string",
                            @enum = new[] { "most", "least" },
                            description = "'most' = most expensive first, 'least' = cheapest first"
                        }),
                        ["count"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "integer",
                            description = "How many products to return (1-20). Default 5."
                        }),
                    },
                    Required = ["order"],
                },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                // Read what Claude asked for, with safe defaults.
                var order = toolUse.Input.TryGetValue("order", out var o) ? o.GetString() : "most";
                var count = toolUse.Input.TryGetValue("count", out var c) && c.TryGetInt32(out var n) ? n : 5;
                count = Math.Clamp(count, 1, 20);

                var query = _db.Products
                    .Where(p => p.Discontinued == 0 && !p.IsDeleted && p.UnitPrice != null);

                query = order == "least"
                    ? query.OrderBy(p => p.UnitPrice)
                    : query.OrderByDescending(p => p.UnitPrice);

                var items = await query
                    .Take(count)
                    .Select(p => new { p.ProductName, p.UnitPrice, p.UnitsInStock })
                    .ToListAsync(ct);

                return JsonSerializer.Serialize(items);
            },ct)
        };

        var lateOrdersTool = new BetaRunnableTool
        {
            Name = "get_late_orders",
            Definition = new BetaTool
            {
                Name = "get_late_orders",
                Description = "Finds orders that shipped after their required date, worst first. " +
                              "Also counts orders that have not shipped yet.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["count"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "integer",
                            description = "How many of the worst late orders to list (1-20). Default 10."
                        }),
                    },
                },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                var count = toolUse.Input.TryGetValue("count", out var c) && c.TryGetInt32(out var n) ? n : 10;
                count = Math.Clamp(count, 1, 20);

                // The database finds the late orders...
                var late = await _db.Orders
                    .Where(o => o.ShippedDate != null && o.RequiredDate != null
                             && o.ShippedDate > o.RequiredDate)
                    .Select(o => new { o.OrderId, o.CustomerId, o.ShipCountry, o.RequiredDate, o.ShippedDate })
                    .ToListAsync(ct);

                var unshipped = await _db.Orders.CountAsync(o => o.ShippedDate == null, ct);

                // ...then C# works out how many days late each one was.
                var worst = late
                    .Select(o => new
                    {
                        o.OrderId,
                        o.CustomerId,
                        o.ShipCountry,
                        o.RequiredDate,
                        o.ShippedDate,
                        DaysLate = o.ShippedDate!.Value.DayNumber - o.RequiredDate!.Value.DayNumber
                    })
                    .OrderByDescending(o => o.DaysLate)
                    .Take(count);

                return JsonSerializer.Serialize(new
                {
                    TotalLateOrders = late.Count,
                    UnshippedOrders = unshipped,
                    WorstLateOrders = worst
                });
            }, ct),
        };

        var bestSellersTool = new BetaRunnableTool
        {
            Name = "get_best_sellers",
            Definition = new BetaTool
            {
                Name = "get_best_sellers",
                Description = "Ranks products by how much they have sold, either by total units sold " +
                              "or by total revenue (after discounts).",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["rank_by"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "string",
                            @enum = new[] { "units", "revenue" },
                            description = "'units' = most items sold, 'revenue' = most money brought in"
                        }),
                        ["count"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "integer",
                            description = "How many products to return (1-20). Default 5."
                        }),
                    },
                    Required = ["rank_by"],
                },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                var rankBy = toolUse.Input.TryGetValue("rank_by", out var r) ? r.GetString() : "units";
                var count = toolUse.Input.TryGetValue("count", out var c) && c.TryGetInt32(out var n) ? n : 5;
                count = Math.Clamp(count, 1, 20);

                // Add up every order line, grouped by product.
                var sales = _db.OrderDetails
                    .GroupBy(od => od.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        UnitsSold = g.Sum(od => (int)od.Quantity),
                        Revenue = g.Sum(od => (double)od.UnitPrice * od.Quantity * (1 - (double)od.Discount))
                    });

                sales = rankBy == "revenue"
                    ? sales.OrderByDescending(s => s.Revenue)
                    : sales.OrderByDescending(s => s.UnitsSold);

                var top = await sales.Take(count).ToListAsync(ct);

                // Look up the names for just those products.
                var ids = top.Select(s => s.ProductId).ToList();
                var names = await _db.Products
                    .Where(p => ids.Contains(p.ProductId))
                    .ToDictionaryAsync(p => p.ProductId, p => p.ProductName, ct);

                var result = top.Select(s => new
                {
                    ProductName = names.GetValueOrDefault(s.ProductId, "Unknown"),
                    s.UnitsSold,
                    Revenue = Math.Round(s.Revenue, 2)
                });

                return JsonSerializer.Serialize(result);
            },ct),
        };

        var emailLowStockTool = new BetaRunnableTool
        {
            Name = "email_lowest_stock_report",
            Definition = new BetaTool
            {
                Name = "email_lowest_stock_report",
                Description = "Finds the active products with the fewest units in stock, builds a CSV " +
                              "of them, and emails it to the store owner with the list in the email body. " +
                              "Use when the user wants to order or restock the lowest items.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["count"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "integer",
                            description = "How many of the lowest-stock products to include (1-10). Default 3."
                        }),
                    },
                },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                if (string.IsNullOrWhiteSpace(_reportEmail))
                    return "No report email is configured, so nothing was sent.";

                var count = toolUse.Input.TryGetValue("count", out var c) && c.TryGetInt32(out var n) ? n : 3;
                count = Math.Clamp(count, 1, 10);

                // 1. Find the lowest-stock products.
                var lowest = await _db.Products
                    .Where(p => p.Discontinued == 0 && !p.IsDeleted)
                    .OrderBy(p => p.UnitsInStock ?? 0)
                    .Take(count)
                    .ToListAsync(ct);

                // 2. Turn them into the DTO your CSV service expects.
                var dtos = lowest.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit ?? "",
                    UnitPrice = (decimal?)p.UnitPrice,
                    UnitsInStock = p.UnitsInStock
                }).ToList();

                var csvBytes = _csv.GenerateProductsCsv(dtos);

                // 3. Build the email body as a simple HTML list.
                var items = string.Join("", dtos.Select(d =>
                    $"<li><strong>{WebUtility.HtmlEncode(d.ProductName)}</strong>: " +
                    $"{d.UnitsInStock ?? 0} in stock, ${d.UnitPrice:0.00} each</li>"));

                var body = $"<h2>Restock list: {dtos.Count} lowest-stock products</h2><ul>{items}</ul>" +
                           "<p>The full details are attached as a CSV.</p>";

                // 4. Send it, and tell Claude whether it worked.
                try
                {
                    await _email.SendEmailAsync(
                        _reportEmail,
                        $"Northwind restock list ({DateTime.Now:MMM d})",
                        body,
                        csvBytes,
                        $"restock-{DateTime.Now:yyyy-MM-dd}.csv");

                    return JsonSerializer.Serialize(new
                    {
                        EmailSent = true,
                        Products = dtos.Select(d => new { d.ProductName, d.UnitsInStock })
                    });
                }
                catch (Exception ex)
                {
                    return $"The email failed to send: {ex.Message}";
                }
            }, ct),
        };

        var vendorRegionsTool = new BetaRunnableTool
        {
            Name = "get_vendor_success_by_country",
            Definition = new BetaTool
            {
                Name = "get_vendor_success_by_country",
                Description = "Ranks countries by how successful their suppliers (vendors) are, based on " +
                              "revenue from those suppliers' products. Includes each country's top supplier.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["rank_by"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "string",
                            @enum = new[] { "total", "average" },
                            description = "'total' = most revenue overall, 'average' = most revenue per supplier"
                        }),
                        ["count"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "integer",
                            description = "How many countries to return (1-20). Default 5."
                        }),
                    },
                    Required = ["rank_by"],
                },
            },
            Run = async (toolUse, ct) => await OneAtATime(async () =>
            {
                var rankBy = toolUse.Input.TryGetValue("rank_by", out var r) ? r.GetString() : "total";
                var count = toolUse.Input.TryGetValue("count", out var c) && c.TryGetInt32(out var n) ? n : 5;
                count = Math.Clamp(count, 1, 20);

                // 1. Connect each sale to its product's supplier, then total sales per supplier.
                var supplierSales = await _db.OrderDetails
                    .Join(_db.Products,
                          od => od.ProductId,
                          p => p.ProductId,
                          (od, p) => new
                          {
                              p.SupplierId,
                              Revenue = (double)od.UnitPrice * od.Quantity * (1 - (double)od.Discount)
                          })
                    .Where(x => x.SupplierId != null)
                    .GroupBy(x => x.SupplierId)
                    .Select(g => new { SupplierId = g.Key!.Value, Revenue = g.Sum(x => x.Revenue) })
                    .ToListAsync(ct);

                // 2. Look up supplier names and countries.
                var suppliers = await _db.Suppliers.ToDictionaryAsync(s => s.SupplierId, ct);

                // 3. Group the suppliers by country and score each country.
                var byCountry = supplierSales
                    .Where(s => suppliers.ContainsKey(s.SupplierId))
                    .GroupBy(s => suppliers[s.SupplierId].Country ?? "Unknown")
                    .Select(g =>
                    {
                        var top = g.OrderByDescending(s => s.Revenue).First();
                        return new
                        {
                            Country = g.Key,
                            Suppliers = g.Count(),
                            TotalRevenue = Math.Round(g.Sum(s => s.Revenue), 2),
                            AveragePerSupplier = Math.Round(g.Average(s => s.Revenue), 2),
                            TopSupplier = suppliers[top.SupplierId].CompanyName
                        };
                    });

                var ranked = rankBy == "average"
                    ? byCountry.OrderByDescending(x => x.AveragePerSupplier)
                    : byCountry.OrderByDescending(x => x.TotalRevenue);

                return JsonSerializer.Serialize(ranked.Take(count));
            }, ct),
        };

        // Names of the other tools, so the email shows what the agent had to work with.
        var otherToolNames = string.Join(", ",
            new[] { lowStockTool, priceRankTool, lateOrdersTool, bestSellersTool, emailLowStockTool, vendorRegionsTool }
                .Select(t => t.Name));

        var unansweredTool = new BetaRunnableTool
        {
            Name = "report_unanswered_question",
            Definition = new BetaTool
            {
                Name = "report_unanswered_question",
                Description = "Emails the store owner a question you couldn't answer with your other tools, " +
                              "so a tool can be added for it later. Use only when no other tool can answer.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["reason"] = JsonSerializer.SerializeToElement(new
                        {
                            type = "string",
                            description = "A short reason why you couldn't answer the question."
                        }),
                    },
                    Required = ["reason"],
                },
            },
            Run = async (toolUse, ct) =>
            {
                if (string.IsNullOrWhiteSpace(_reportEmail))
                    return "No report email is configured, so nothing was sent.";

                var reason = toolUse.Input.TryGetValue("reason", out var r) ? r.GetString() : "No reason given";

                var body = "<h2>Unanswered question</h2>" +
                           $"<p><strong>Question:</strong> {WebUtility.HtmlEncode(question)}</p>" +
                           $"<p><strong>Why:</strong> {WebUtility.HtmlEncode(reason)}</p>" +
                           $"<p><strong>When:</strong> {DateTime.UtcNow:MMM d, yyyy h:mm tt} UTC</p>" +
                           $"<p><strong>Tools it had:</strong> {otherToolNames}</p>";

                try
                {
                    await _email.SendEmailAsync(_reportEmail, "Northwind agent: unanswered question", body);
                    return "Question reported.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unanswered-question email failed: {ex.Message}");
                    return $"The report failed to send: {ex.Message}";
                }
            },
        };

        // The runner handles the loop: Claude asks for a tool, we run it, Claude answers.
        var runner = _client.Beta.Messages.ToolRunner(
            new MessageCreateParams
            {
                Model = "claude-haiku-4-5",
                MaxTokens = 1024,
                System = "You are a restock assistant for the Northwind store. " +
                 "Use your tools to check real inventory data. Never guess numbers. " +
                 "If the user asks something you can't answer with your tools, don't guess. " +
                 "Call report_unanswered_question with the question and a short reason why " +
                 "you couldn't answer it. Then tell the user: " +
                 "\"I can't answer that yet, but I've passed your question along.\"",
                Messages = [new() { Role = Role.User, Content = question }],
            },
            [lowStockTool, priceRankTool, lateOrdersTool, bestSellersTool, emailLowStockTool, vendorRegionsTool, unansweredTool]
        );

        var answer = "";
        await foreach (var message in runner)
        {
            answer = "";
            foreach (var block in message.Content)
            {
                if (block.TryPickText(out var text))
                    answer += text.Text;
            }
        }

        return answer;
    }

}