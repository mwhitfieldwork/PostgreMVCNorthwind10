using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure;



namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class OrderHistoryService : GenericService<OrderDetailsExtended>, IOrderHistory
    {
        private readonly PgNwContext _dc;
        public OrderHistoryService(PgNwContext dc) : base(dc)
        {
            this._dc = dc;
        }

        public async Task<List<OrderDetailsExtended>> GetOrderHistory()
        {
            return await _dc.OrderDetails
            .Join(_dc.Products,
                  o => o.ProductId,
                  p => p.ProductId,
                  (o, p) => new OrderDetailsExtended
                  {
                      OrderId = o.OrderId,
                      ProductId = p.ProductId,
                      UnitPrice = o.UnitPrice,
                      Discount = o.Discount,
                      ProductName = p.ProductName, // Directly get the ProductName from the join
                      Quantity = o.Quantity
                  })
            .Take(25)
            .ToListAsync();
        }
    }
}
