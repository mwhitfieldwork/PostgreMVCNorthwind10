using System;
using System.Collections.Generic;
using System.Text;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    /// <summary>
    /// WHAT: One candlestick for one product for one week
    ///       (open, high, low, close daily sales totals).
    /// WHY:  The dashboard's D3 candlestick chart needs these four numbers
    ///       per week. Storing them means the API doesn't recalculate
    ///       every time the chart loads.
    /// HOW:  EF Core maps this class to the product_sales_candles table.
    ///       Each row links to a product through ProductId (foreign key).
    ///       Column names and rules are set up in PgNwContext.
    /// </summary>
    public class ProductSalesCandle
    {
        // Primary key: unique id for each candle row
        public int Id { get; set; }

        // Foreign key: which product this candle belongs to
        // NOTE: must be the same type as Product.ProductId (see below)
        public short ProductId { get; set; }

        // The Monday (or first day) of the week this candle covers
        public DateOnly WeekStart { get; set; }

        // Sales total on the first day with orders that week
        public decimal Open { get; set; }

        // Highest single-day sales total that week
        public decimal High { get; set; }

        // Lowest single-day sales total that week
        public decimal Low { get; set; }

        // Sales total on the last day with orders that week
        public decimal Close { get; set; }

        // Navigation property: lets EF Core load the related Product
        public Product Product { get; set; } = null!;
    }
}
