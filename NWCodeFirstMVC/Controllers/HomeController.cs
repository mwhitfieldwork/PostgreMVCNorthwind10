using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Models;
// using NWCodeFirstMVC.Domain.Models; (removed - scaffold)
using System.Diagnostics;
using NWCodeFirstMVC.Domain;

namespace NWCodeFirstMVC.Controllers
{
    public class HomeController : Controller
    {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}