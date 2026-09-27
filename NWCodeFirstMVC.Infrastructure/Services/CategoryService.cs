using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using System.Data;
using NWCodeFirstMVC.Domain.Contracts;
using DomainCategory = NWCodeFirstMVC.Domain.PocoModels.Category;
using PgCategory = NWCodeFirstMVC.Infrastructure.PgModels.Category;
using AutoMapper;




namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class CategoryService:GenericService<Category>, ICategoryService
    {
        private readonly PgNwContext _dc;
        private readonly IMapper _mapper;
        public CategoryService(PgNwContext dc, IMapper mapper) : base(dc)
        {
            this._dc = dc;
            _mapper = mapper;

        }

        public override async Task<List<DomainCategory>> GetAllAsync()
        {
            var pgCategories = await _dc.Categories.ToListAsync(); // PgModels.Category
            return _mapper.Map<List<DomainCategory>>(pgCategories);
        }



        public async Task<List<SalesByCategoryDTO>> GetSalesByCategory(string categoryName, string orderYear)
        {
            if (orderYear != "1996" && orderYear != "1997" && orderYear != "1998")
            {
                orderYear = "1998";
            }

            var query =
                from od in _dc.OrderDetails
                join o in _dc.Orders on od.OrderId equals o.OrderId
                join p in _dc.Products on od.ProductId equals p.ProductId
                join c in _dc.Categories on p.CategoryId equals c.CategoryId
                where c.CategoryName == categoryName
                      && o.OrderDate.Year.ToString() == orderYear
                group new { od, p } by p.ProductName into g
                orderby g.Key
                select new SalesByCategoryDTO
                {
                    ProductId = g.First().od.ProductId,
                    ProductName = g.Key,
                    TotalPurchase = Math.Round(
                        g.Sum(x => x.od.Quantity * (1 - x.od.Discount) * x.od.UnitPrice),
                        0
                    ).ToString()
                };

            return await query.ToListAsync();
        }


        public async Task<List<CustomerOrderDTO>> GetCustomerOrders(string customerId)
        {
            try
            {
                var customerOrders = await _dc.OrderDetails
                    .Join(_dc.Products,
                        o => o.ProductId,
                        p => p.ProductId,
                        (o, p) => new { o, p })
                    .Join(_dc.Orders,
                        temp => temp.o.OrderId,
                        od => od.OrderId,
                        (temp, od) => new { temp.o, temp.p, od })
                    .Join(_dc.Customers,
                        temp => temp.od.CustomerId,
                        c => c.CustomerId,
                        (temp, c) => new { temp.o, temp.p, temp.od, c })
                    .Join(_dc.Categories,
                        temp => temp.p.CategoryId,
                        ca => ca.CategoryId,
                        (temp, ca) => new CustomerOrderDTO
                        {
                            CustomerID = temp.c.CustomerId,
                            ProductName = temp.p.ProductName,
                            ProductID = temp.p.ProductId,
                            CategoryName = ca.CategoryName,
                            Quantity = temp.o.Quantity,
                            UnitPrice = temp.o.UnitPrice,
                            OrderDate = temp.od.OrderDate
                        })
                    .Where(result => result.CustomerID == customerId) // Filter by CustomerID
                    .ToListAsync();

                return customerOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customer orders", ex);
            }
        }

        public async Task<List<DistinctCustomerDTO>> GetTopCustomersAsync()
        {
            try
            {
                var customers = await _dc.Customers
                    .GroupBy(c => new { c.CustomerId, c.ContactName }) // Group by CustomerID and ContactName
                    .Select(group => new DistinctCustomerDTO
                    {
                        CustomerID = group.Key.CustomerId,
                        ContactName = group.Key.ContactName
                    })
                    .Take(15) // Fetch the top 15 records
                    .ToListAsync();

                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customer data", ex);
            }
        }
    }

}
