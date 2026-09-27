
using AutoMapper;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure.PgModels;
using NWCodeFirstMVC.Infrastructure.Repositories;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(ProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<ProductModel>>(entities);
        }

        public async Task<ProductModel?> GetAsync(int id)
        {
            var entity = await _repo.GetAsync(id);
            return _mapper.Map<ProductModel>(entity);
        }

        public async Task<ProductModel> AddAsync(ProductModel model)
        {
            var entity = _mapper.Map<Product>(model);
            await _repo.AddAsync(entity);
            return _mapper.Map<ProductModel>(entity);
        }

        public async Task UpdateAsync(ProductModel model)
        {

            var entity = _mapper.Map<Product>(model);
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }

}
