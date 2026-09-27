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
    }
}
