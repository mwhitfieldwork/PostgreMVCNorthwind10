using System;
using System.Collections.Generic;
using System.Text;

namespace NWCodeFirstMVC.Domain.Dto
{
    /// <summary>
    /// WHAT: One weekly candle sent to the Angular candlestick chart.
    /// WHY:  Keeps the API response simple and separate from the database class.
    /// HOW:  Filled by DashboardService from the product_sales_candles table.
    ///       WeekStart is sent as "yyyy-MM-dd" in the JSON.
    /// </summary>
    public class ProductSalesCandleDto
    {
        public DateOnly WeekStart { get; set; }  // Monday of the week
        public decimal Open { get; set; }        // first day's sales
        public decimal High { get; set; }        // best day's sales
        public decimal Low { get; set; }         // worst day's sales
        public decimal Close { get; set; }       // last day's sales
    }
}
