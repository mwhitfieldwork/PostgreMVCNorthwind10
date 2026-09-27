using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetAsync(int id);
        Task<ProductModel> AddAsync(ProductModel model);
        Task UpdateAsync(ProductModel model);
        Task DeleteAsync(int id);
    }
}
