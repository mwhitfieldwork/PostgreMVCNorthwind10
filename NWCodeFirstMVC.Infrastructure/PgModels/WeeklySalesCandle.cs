using System;
using System.Collections.Generic;
using System.Text;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    /// <summary>
    /// WHAT: One candlestick for ALL sales combined for one week
    ///       (open, high, low, close daily sales totals).
    /// WHY:  The dashboard's D3 candlestick chart shows company-wide sales,
    ///       which has data almost every week (full candles, few gaps).
    /// HOW:  EF Core maps this class to the weekly_sales_candles table.
    ///       One row per week (week_start is unique).
    ///       Column names and rules are set up in PgNwContext.
    /// </summary>
    public class WeeklySalesCandle
    {
        // Primary key: unique id for each candle row
        public int Id { get; set; }

        // The Monday of the week this candle covers
        public DateOnly WeekStart { get; set; }

        // Total sales on the first day with orders that week
        public decimal Open { get; set; }

        // Highest single-day sales total that week
        public decimal High { get; set; }

        // Lowest single-day sales total that week
        public decimal Low { get; set; }

        // Total sales on the last day with orders that week
        public decimal Close { get; set; }
    }
}
