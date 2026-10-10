using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class InvoiceService:IInvoiceService
    {  
        private readonly IEmailService _emailService;
        private readonly ICsvService _csvService;
        private readonly PgNwContext _db;


        public InvoiceService(IEmailService emailService, ICsvService csvService, PgNwContext db)
        {
            _emailService = emailService;
            _csvService = csvService;
            _db = db;
        }


        public async Task SendInvoiceAsync(string email, List<int> productIds)
        {
            var products = await _db.Products
                .Where(p => productIds.Contains(p.ProductId) && !p.IsDeleted)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit ?? "",
                    UnitPrice = (decimal?)p.UnitPrice,
                    UnitsInStock = p.UnitsInStock
                })
                .ToListAsync();

            if (products.Count == 0)
                throw new InvalidOperationException("None of the selected products were found.");

            var csvBytes = _csvService.GenerateProductsCsv(products);

            var items = string.Join("", products.Select(p =>
                $"<li>{WebUtility.HtmlEncode(p.ProductName)} — ${p.UnitPrice:0.00}</li>"));

            await _emailService.SendEmailAsync(
                email,
                "Your Invoice",
                $"<h2>Your Invoice</h2><ul>{items}</ul>",
                csvBytes,
                "invoice.csv");
        }
    }
}
