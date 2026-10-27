using Microsoft.CodeAnalysis.CSharp.Syntax;
using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Infrastructure.PgModels;


namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class DashboardService : GenericService<SalesByCategory>, IDashboardService
    {
        private readonly PgNwContext _dc;
        public DashboardService(PgNwContext dc) : base(dc)
        {
            this._dc = dc;
        }


        public async Task<List<TopCardTotalDTO>> GetAllTopCardTotals()
        {
            try
            {
                var cardTotals = await _dc.OrderDetails
                    .OrderByDescending(o => o.UnitPrice)
                    .Take(4)
                    .Select(o => new TopCardTotalDTO
                    {
                        OrderID = o.OrderId,
                        UnitPrice = o.UnitPrice
                    })
                    .ToListAsync();

                return cardTotals;
            }
            catch (Exception ex)
            {
                // Log the error however your app logs (Serilog, ILogger, etc.)
                Console.WriteLine($"DashboardService.GetAllTopCardTotals ERROR: {ex.Message}");

                return new List<TopCardTotalDTO>(); // prevents Swagger from blowing up
            }
        }


        public async Task<List<TotalSalesCategoryDTO>> GetAllSalesTotals(
            DateTime beginningDate,
            DateTime endingDate)
        {
            var begin = DateOnly.FromDateTime(beginningDate);
            var end = DateOnly.FromDateTime(endingDate);

            var results =
                from e in _dc.Employees
                join o in _dc.Orders on e.EmployeeId equals o.EmployeeId
                where o.ShippedDate >= begin && o.ShippedDate <= end
                select new TotalSalesCategoryDTO
                {
                    Country = e.Country,
                    OrderId = o.OrderId,
                    SaleAmount = _dc.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Sum(od => od.UnitPrice * od.Quantity)
                };

            return await results.Take(4).ToListAsync();
        }


        public async Task<List<SalesLineDTO>> GetSalesByDateRange(
            DateTime beginningDate,
            DateTime endingDate)
        {
            var begin = DateOnly.FromDateTime(beginningDate);
            var end = DateOnly.FromDateTime(endingDate);

            var results =
                from o in _dc.Orders
                join c in _dc.Customers on o.CustomerId equals c.CustomerId
                join od in _dc.OrderDetails on o.OrderId equals od.OrderId
                join p in _dc.Products on od.ProductId equals p.ProductId
                join cat in _dc.Categories on p.CategoryId equals (int?)cat.CategoryId
                join s in _dc.Shippers on o.ShipVia equals (int?)s.ShipperId
                where o.OrderDate >= begin && o.OrderDate <= end
                orderby o.OrderDate descending, o.OrderId
                select new SalesLineDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToDateTime(TimeOnly.MinValue),
                    CustomerId = c.CustomerId,
                    CustomerName = c.CompanyName,
                    CustomerCountry = c.Country,
                    CategoryName = cat.CategoryName,
                    ProductName = p.ProductName,
                    ShipperName = s.CompanyName,
                    Quantity = od.Quantity,
                    UnitPrice = (decimal)od.UnitPrice,
                    Discount = (decimal)od.Discount,
                    LineTotal = Math.Round((decimal)(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2)
                };

            return await results.ToListAsync();
        }


        public async Task<List<SalesLineDTO>> GetSalesByDateRange()
        {

            var results =
                from o in _dc.Orders
                join c in _dc.Customers on o.CustomerId equals c.CustomerId
                join od in _dc.OrderDetails on o.OrderId equals od.OrderId
                join p in _dc.Products on od.ProductId equals p.ProductId
                join cat in _dc.Categories on p.CategoryId equals (int?)cat.CategoryId
                join s in _dc.Shippers on o.ShipVia equals (int?)s.ShipperId
                orderby o.OrderDate descending, o.OrderId
                select new SalesLineDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToDateTime(TimeOnly.MinValue),
                    CustomerId = c.CustomerId,
                    CustomerName = c.CompanyName,
                    CustomerCountry = c.Country,
                    CategoryName = cat.CategoryName,
                    ProductName = p.ProductName,
                    ShipperName = s.CompanyName,
                    Quantity = od.Quantity,
                    UnitPrice = (decimal)od.UnitPrice,
                    Discount = (decimal)od.Discount,
                    LineTotal = Math.Round((decimal)(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2)
                };

            return await results.ToListAsync();
        }

        /// <summary>
        /// WHAT: Rebuilds the product_sales_candles table from Northwind's orders.
        /// WHY:  The D3 candlestick chart needs open/high/low/close per product per week.
        ///       Calculating once and storing it keeps the chart's API call fast.
        /// HOW:  1) Postgres adds up each product's sales per day
        ///       2) C# groups those days into Monday-start weeks
        ///       3) For each week: first day = open, best = high, worst = low, last = close
        ///       4) Old candles are deleted and new ones saved, all-or-nothing (transaction)
        /// </summary>
        public async Task<int> RebuildProductSalesCandles()
        {
            try
            {
                // 1) Daily sales total per product (done in the database)
                //    Sale amount = price × quantity × (1 - discount)
                var dailyTotals = await _dc.OrderDetails
                    .GroupBy(od => new { od.ProductId, od.Order.OrderDate })
                    .Select(g => new
                    {
                        g.Key.ProductId,
                        g.Key.OrderDate,
                        Total = g.Sum(od => od.UnitPrice * od.Quantity * (1 - od.Discount))
                    })
                    .ToListAsync();

                // 2 + 3) Group days into weeks and build one candle per product per week
                var candles = dailyTotals
                    .GroupBy(d => new { d.ProductId, WeekStart = GetWeekStart(d.OrderDate) })
                    .Select(week =>
                    {
                        // Put the days in date order so first = open and last = close
                        var days = week.OrderBy(d => d.OrderDate).ToList();

                        return new ProductSalesCandle
                        {
                            ProductId = week.Key.ProductId,
                            WeekStart = week.Key.WeekStart,
                            Open = ToMoney(days.First().Total),
                            High = ToMoney(days.Max(d => d.Total)),
                            Low = ToMoney(days.Min(d => d.Total)),
                            Close = ToMoney(days.Last().Total)
                        };
                    })
                    .ToList();

                // 4) Replace old candles with new ones.
                //    The transaction means: if anything fails, nothing changes.
                await using var transaction = await _dc.Database.BeginTransactionAsync();

                await _dc.ProductSalesCandles.ExecuteDeleteAsync(); // clear the table
                _dc.ProductSalesCandles.AddRange(candles);
                await _dc.SaveChangesAsync();

                await transaction.CommitAsync();

                return candles.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DashboardService.RebuildProductSalesCandles ERROR: {ex.Message}");
                throw; // let the controller report the failure instead of hiding it
            }
        }

        // Finds the Monday on or before the given date (the start of that week)
        private static DateOnly GetWeekStart(DateOnly date)
        {
            // DayOfWeek: Sunday = 0, Monday = 1 ... this turns it into "days since Monday"
            int daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-daysSinceMonday);
        }
        /// <summary>
        /// WHAT: Gets weekly candles for one product, for the candlestick chart.
        /// WHY:  Angular sends either a date range (from the date picker) or nothing
        ///       (first load), and needs the right weeks back either way.
        /// HOW:  - end   = picker's end date, or the product's latest week with data
        ///       - start = picker's start date, or "weeks" weeks before end
        ///       - start is moved back to its Monday so a partly-picked week still shows
        ///       - returns candles oldest → newest (the order the chart draws them)
        /// </summary>
        public async Task<List<ProductSalesCandleDto>> GetProductSalesCandles(
            short productId, DateOnly? start, DateOnly? end, int weeks)
        {
            try
            {
                // Safety: always ask for at least 1 week
                if (weeks < 1) weeks = 1;

                // All candles for this product (not run yet, just being built up)
                var query = _dc.ProductSalesCandles.Where(c => c.ProductId == productId);

                // No end date? Use this product's most recent week with data.
                // (Northwind's orders are from 1996–98, so "today" would show nothing.)
                if (end == null)
                {
                    end = await query.MaxAsync(c => (DateOnly?)c.WeekStart);

                    // Product has no candles at all
                    if (end == null) return new List<ProductSalesCandleDto>();
                }

                // No start date? Count back from the end date.
                // e.g. 26 weeks = the end week plus the 25 weeks before it
                start ??= end.Value.AddDays(-7 * (weeks - 1));

                // Snap start back to its Monday so the week it falls in is included
                start = GetWeekStart(start.Value);

                return await query
                    .Where(c => c.WeekStart >= start && c.WeekStart <= end)
                    .OrderBy(c => c.WeekStart)
                    .Select(c => new ProductSalesCandleDto
                    {
                        WeekStart = c.WeekStart,
                        Open = c.Open,
                        High = c.High,
                        Low = c.Low,
                        Close = c.Close
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DashboardService.GetProductSalesCandles ERROR: {ex.Message}");
                return new List<ProductSalesCandleDto>();
            }
        }


        /// <summary>
        /// WHAT: Rebuilds the weekly_sales_candles table (all products combined).
        /// WHY:  The dashboard chart shows company-wide sales, which has orders
        ///       almost every week, so the default view shows a full set of candles.
        /// HOW:  1) Postgres adds up ALL sales per day
        ///       2) C# groups those days into Monday-start weeks
        ///       3) For each week: first day = open, best = high, worst = low, last = close
        ///       4) Old candles are deleted and new ones saved, all-or-nothing (transaction)
        /// </summary>
        public async Task<int> RebuildWeeklySalesCandles()
        {
            try
            {
                // 1) Total sales per day, all products together (done in the database)
                var dailyTotals = await _dc.OrderDetails
                    .GroupBy(od => od.Order.OrderDate)
                    .Select(g => new
                    {
                        OrderDate = g.Key,
                        Total = g.Sum(od => od.UnitPrice * od.Quantity * (1 - od.Discount))
                    })
                    .ToListAsync();

                // 2 + 3) Group days into weeks and build one candle per week
                var candles = dailyTotals
                    .GroupBy(d => GetWeekStart(d.OrderDate))
                    .Select(week =>
                    {
                        // Date order, so first = open and last = close
                        var days = week.OrderBy(d => d.OrderDate).ToList();

                        return new WeeklySalesCandle
                        {
                            WeekStart = week.Key,
                            Open = ToMoney(days.First().Total),
                            High = ToMoney(days.Max(d => d.Total)),
                            Low = ToMoney(days.Min(d => d.Total)),
                            Close = ToMoney(days.Last().Total)
                        };
                    })
                    .ToList();

                // 4) Replace old candles with new ones (if anything fails, nothing changes)
                await using var transaction = await _dc.Database.BeginTransactionAsync();

                await _dc.WeeklySalesCandles.ExecuteDeleteAsync();
                _dc.WeeklySalesCandles.AddRange(candles);
                await _dc.SaveChangesAsync();

                await transaction.CommitAsync();

                return candles.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DashboardService.RebuildWeeklySalesCandles ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// WHAT: Gets weekly candles for all sales combined, for the candlestick chart.
        /// WHY:  Angular sends a date range (from the dashboard picker) or nothing (first load).
        /// HOW:  - end   = picker's end date, or the latest week with data
        ///       - start = picker's start date, or "weeks" weeks before end
        ///       - start snaps back to its Monday so a partly-picked week still shows
        ///       - returns candles oldest → newest
        /// </summary>
        public async Task<List<ProductSalesCandleDto>> GetWeeklySalesCandles(
            DateOnly? start, DateOnly? end, int weeks)
        {
            try
            {
                if (weeks < 1) weeks = 1;

                var query = _dc.WeeklySalesCandles.AsQueryable();

                // No end date? Use the most recent week with data (May 1998)
                if (end == null)
                {
                    end = await query.MaxAsync(c => (DateOnly?)c.WeekStart);
                    if (end == null) return new List<ProductSalesCandleDto>();
                }

                // No start date? Count back from the end date
                start ??= end.Value.AddDays(-7 * (weeks - 1));
                start = GetWeekStart(start.Value);

                return await query
                    .Where(c => c.WeekStart >= start && c.WeekStart <= end)
                    .OrderBy(c => c.WeekStart)
                    .Select(c => new ProductSalesCandleDto
                    {
                        WeekStart = c.WeekStart,
                        Open = c.Open,
                        High = c.High,
                        Low = c.Low,
                        Close = c.Close
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DashboardService.GetWeeklySalesCandles ERROR: {ex.Message}");
                return new List<ProductSalesCandleDto>();
            }
        }

        // Converts the float total to decimal, rounded to cents
        private static decimal ToMoney(float value) => Math.Round((decimal)value, 2);
    }
}
