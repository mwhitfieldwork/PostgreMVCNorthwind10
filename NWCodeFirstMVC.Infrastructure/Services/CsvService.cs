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
                sb.AppendLine($"{p.ProductName},{p.QuantityPerUnit},{p.UnitPrice},{p.UnitsInStock}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
