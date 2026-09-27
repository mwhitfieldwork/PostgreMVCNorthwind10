using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
            {
                try
                {
                    var products = await _productService.GetAllAsync();
                    var productsDto = mapper.Map<List<GetProductDto>>(products);
                    return Ok(productsDto);
                }
                catch (Exception ex)
                {
                    // Log the exception (Serilog, ILogger, etc.)
                    //ILogger.LogError(ex, "Failed to retrieve products");

                    return StatusCode(500, "An unexpected error occurred while retrieving products.");
                }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAProduct(int id)
        {
            try
            {
                var product = await _productService.GetAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                var productDto = mapper.Map<GetProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving products.");
            }
        }

        /*
        [HttpGet("GetLuxUsProd")]
        public IActionResult GetLuxuryUSProduct() //IActionResult returns the json data
        {
            //returning a response ( ok, 404, ect) 
            // use the inteface which is used a  service
            return Ok(_productService.GetLuxuryUSProduct()); 
        }

        [HttpGet("GetProdOrders")]
        public IActionResult GetProdOrders() //IActionResult returns the json data
        {
            //returning a response ( ok, 404, ect) 
            // use the inteface which is used a  service
            var results = _productService.GetProductWithHighQuantityOrders();

            return Ok(results);
        }*/

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDto createproduct)
        {
            var product = mapper.Map<ProductModel>(createproduct);
            var result = await _productService.AddAsync(product);
            return CreatedAtAction("GetAllProduct", new { ProductId = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            if (id != updateProductDto.ProductId)
            {
                return BadRequest("Invalid ID");
            }
            var product = await _productService.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            var existingProduct = mapper.Map(updateProductDto, product);
            try
            {
                await _productService.UpdateAsync(existingProduct);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (product.ProductId != id)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);

            return NoContent();

        }
    }
}
