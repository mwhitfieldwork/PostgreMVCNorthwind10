using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface ICategoryService : IGenericRepository<Category>
    {
        Task<List<SalesByCategoryDTO>> GetSalesByCategory(string categoryName, string orderYear);

        Task<List<CustomerOrderDTO>> GetCustomerOrders(string customerId);

        Task<List<DistinctCustomerDTO>> GetTopCustomersAsync();
    }
}
