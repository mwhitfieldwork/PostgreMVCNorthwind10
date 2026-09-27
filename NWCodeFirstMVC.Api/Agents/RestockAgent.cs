using System.Text.Json;
using Anthropic;
using Anthropic.Helpers.Beta;
using Anthropic.Models.Beta.Messages;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Infrastructure;

namespace NWCodeFirstMVC.Api.Agents;

public class RestockAgent
{
    private readonly PgNwContext _db;
    private readonly AnthropicClient _client;

    public RestockAgent(PgNwContext db, IConfiguration config)
    {
        _db = db;
        _client = new AnthropicClient { ApiKey = config["Anthropic:ApiKey"] };
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
            Run = async (toolUse, ct) =>
            {
                var items = await _db.Products
                    .Where(p => p.Discontinued == 0 && !p.IsDeleted
                        && (p.UnitsInStock ?? 0) + (p.UnitsOnOrder ?? 0) <= (p.ReorderLevel ?? 0))
                    .Select(p => new { p.ProductName, p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel })
                    .ToListAsync(ct);

                return JsonSerializer.Serialize(items);
            },
        };

        // The runner handles the loop: Claude asks for a tool, we run it, Claude answers.
        var runner = _client.Beta.Messages.ToolRunner(
            new MessageCreateParams
            {
                Model = "claude-haiku-4-5",
                MaxTokens = 1024,
                System = "You are a restock assistant for the Northwind store. " +
                         "Use your tools to check real inventory data. Never guess numbers.",
                Messages = [new() { Role = Role.User, Content = question }],
            },
            [lowStockTool]
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