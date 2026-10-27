using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IDashboardService : IGenericRepository<SalesByCategory>
    {
        Task<List<TopCardTotalDTO>> GetAllTopCardTotals();

        Task<List<TotalSalesCategoryDTO>> GetAllSalesTotals(
            DateTime beginningDate,
            DateTime endingDate);

        Task<List<SalesLineDTO>> GetSalesByDateRange();
        Task<List<SalesLineDTO>> GetSalesByDateRange(DateTime startDate, DateTime endDate);

        // Clears and refills the product_sales_candles table. Returns how many candles were saved.
        Task<int> RebuildProductSalesCandles();

        // Gets weekly candles for one product.
        // If start/end are missing, returns the latest "weeks" weeks that have data.
        Task<List<ProductSalesCandleDto>> GetProductSalesCandles(
            short productId, DateOnly? start, DateOnly? end, int weeks);

        // Clears and refills weekly_sales_candles (all products combined). Returns how many candles were saved.
        Task<int> RebuildWeeklySalesCandles();

        // Gets weekly candles for all sales combined.
        // If start/end are missing, returns the latest "weeks" weeks that have data.
        Task<List<ProductSalesCandleDto>> GetWeeklySalesCandles(DateOnly? start, DateOnly? end, int weeks);
    }
}
