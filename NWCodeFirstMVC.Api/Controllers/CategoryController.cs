using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;

using NWCodeFirstMVC.Domain.Dto;
// removed reference to NWCodeFirstMVC.Domain.Models (SQL Server scaffold)

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }
        // GET: CategoryController
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllAsync();
            var categoriesDto = mapper.Map<List<GetCategoryDto>>(categories);
            return Ok(categoriesDto);
        }

        [HttpGet("{categoryName},{orderYear}")]
        public async Task<IActionResult> GetSalesByCategory(string categoryName, string orderYear)
        {
            var categories = await categoryService.GetSalesByCategory(categoryName, orderYear);
            var categoriesDto = mapper.Map<List<SalesByCategoryDTO>>(categories);
            return Ok(categoriesDto);
        }


        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(string customerId)
        {
            var categoryOrders = await categoryService.GetCustomerOrders(customerId);
            var categoryOrdersDto = mapper.Map<List<CustomerOrderDTO>>(categoryOrders);
            return Ok(categoryOrdersDto);
        }


        [HttpGet("TopCustomers")]
        public async Task<IActionResult> GetTopCustomersAsync()
        {
            var topCustomers = await categoryService.GetTopCustomersAsync();
            var topCustomersDto = mapper.Map<List<DistinctCustomerDTO>>(topCustomers);
            return Ok(topCustomersDto);
        }
       
        /*
        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
