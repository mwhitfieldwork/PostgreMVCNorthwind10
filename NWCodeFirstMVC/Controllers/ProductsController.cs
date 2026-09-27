using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain;
// removed reference to NWCodeFirstMVC.Domain.Models (SQL Server scaffold)
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Domain.Contracts;

namespace NWCodeFirstMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public JsonResult GetAllProducts()
        //{
        //    IQueryable<Product> products = _dc.Products;

        //    var results = products.Select(x =>
        //    new
        //    {
        //        productId = x.ProductId,
        //        ProductName = x.ProductName,
        //        SupplierId = x.SupplierId,
        //        CategoryId = x.CategoryId,
        //        QuantityPerUnit = x.QuantityPerUnit,
        //        UnitPrice = x.UnitPrice,
        //        UnitsInStock = x.UnitsInStock,
        //        UnitsOnOrder = x.UnitsOnOrder,
        //        ReorderLevel = x.ReorderLevel,
        //        Discontinued = x.Discontinued
        //    }).ToList();

        //    return Json(results);
        //}

        public async Task<ActionResult> Index()
        {
            var products = await _service.GetAllAsync();
            return View(products);
        }


        public ActionResult View()
        {
            NWCodeFirstMVC.Domain.PocoModels.Category model = new NWCodeFirstMVC.Domain.PocoModels.Category();

            return View(model);
        }
    }
}
