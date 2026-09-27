using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;

using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Infrastructure.Services;

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        // GET: DashboardController
        private readonly IDashboardService dashboardService;
        private readonly IMapper mapper;

        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            this.dashboardService = dashboardService;
            this.mapper = mapper;
        }
        [HttpGet("totals")]
        public async Task<IActionResult> GetTopCardValues()
        {
            var totals = await dashboardService.GetAllTopCardTotals();
            return Ok(totals);

        }

        [HttpGet("salestotals")]
        public async Task<IActionResult> GetSalesTotalsValues(
        [FromQuery] DateTime beginningDate,
        [FromQuery] DateTime endingDate)
        {
            var totals = await dashboardService.GetAllSalesTotals(beginningDate, endingDate);
            return Ok(totals);
        }

        // GET: CategoryController
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var sales = await dashboardService.GetAllAsync();
            return Ok(sales);
        }

        // GET api/dashboard/sales
        [HttpGet("sales")]
        public async Task<ActionResult<List<SalesLineDTO>>> GetAllSales()
        {
            var results = await dashboardService.GetSalesByDateRange();
            return Ok(results);
        }

        // GET api/dashboard/sales/range?beginningDate=1998-01-01&endingDate=1998-03-31
        [HttpGet("sales/range")]
        public async Task<ActionResult<List<SalesLineDTO>>> GetSalesByDateRange(
            [FromQuery] DateTime beginningDate,
            [FromQuery] DateTime endingDate)
        {
            if (beginningDate > endingDate)
            {
                return BadRequest("beginningDate must be on or before endingDate.");
            }

            var results = await dashboardService.GetSalesByDateRange(beginningDate, endingDate);
            return Ok(results);
        }

    }
}
