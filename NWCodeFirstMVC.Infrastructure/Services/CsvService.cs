using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class CsvService : ICsvService
    {
        public byte[] GenerateProductsCsv(List<ProductDto> products)
        {
            var sb = new StringBuilder();

            // Header row
            sb.AppendLine("ProductId,ProductName,QuantityPerUnit,UnitPrice,UnitsInStock");

            foreach (var p in products)
            {
                // Quotes around the name keep commas inside it from breaking the columns.
                var name = (p.ProductName ?? "").Replace("\"", "\"\"");
                var qty = (p.QuantityPerUnit ?? "").Replace("\"", "\"\"");

                sb.AppendLine($"{p.ProductId},\"{name}\",\"{qty}\",{p.UnitPrice},{p.UnitsInStock}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
