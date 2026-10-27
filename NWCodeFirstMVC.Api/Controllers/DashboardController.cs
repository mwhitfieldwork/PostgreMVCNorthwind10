using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// WHAT: Rebuilds the weekly candlestick data for every product.
        /// WHY:  Run once to fill product_sales_candles, and again whenever orders change.
        /// HOW:  Calls DashboardService.RebuildProductSalesCandles and returns how many
        ///       candles were saved. Requires login so random visitors can't trigger it.
        [HttpPost("candles/rebuild")]
        [Authorize]
        public async Task<IActionResult> RebuildCandles()
        {
            try
            {
                int productCount = await dashboardService.RebuildProductSalesCandles();
                int weeklyCount = await dashboardService.RebuildWeeklySalesCandles();

                return Ok(new { productCandlesSaved = productCount, weeklyCandlesSaved = weeklyCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Candle rebuild failed: {ex.Message}");
            }
        }

        /// <summary>
        /// WHAT: Returns weekly candles for one product, for the D3 candlestick chart.
        /// WHY:  Angular calls this on first load (no dates) and again when the
        ///       date picker changes (with dates).
        /// HOW:  Example calls:
        ///   /Dashboard/candles?productId=1                                → latest 26 weeks
        ///   /Dashboard/candles?productId=1&weeks=12                       → latest 12 weeks (mobile)
        ///   /Dashboard/candles?productId=1&start=1997-01-01&end=1997-03-31 → picker range
        /// </summary>
        [HttpGet("candles")]
        public async Task<IActionResult> GetCandles(
            [FromQuery] short productId,
            [FromQuery] DateOnly? start,
            [FromQuery] DateOnly? end,
            [FromQuery] int weeks = 26)
        {
            var candles = await dashboardService.GetProductSalesCandles(productId, start, end, weeks);
            return Ok(candles);
        }

        /// <summary>
        /// WHAT: Returns weekly candles for all sales combined (the dashboard chart).
        /// HOW:  /Dashboard/candles/weekly                                → latest 26 weeks
        ///       /Dashboard/candles/weekly?weeks=12                       → latest 12 (mobile)
        ///       /Dashboard/candles/weekly?start=1997-01-01&end=1997-03-31 → picker range
        /// </summary>
        [HttpGet("candles/weekly")]
        public async Task<IActionResult> GetWeeklyCandles(
            [FromQuery] DateOnly? start,
            [FromQuery] DateOnly? end,
            [FromQuery] int weeks = 26)
        {
            var candles = await dashboardService.GetWeeklySalesCandles(start, end, weeks);
            return Ok(candles);
        }
    }
}
