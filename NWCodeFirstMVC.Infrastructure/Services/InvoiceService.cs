using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class InvoiceService:IInvoiceService
    {  
        private readonly IEmailService _emailService;

        private readonly ICsvService _csvService;

        public InvoiceService(IEmailService emailService, ICsvService csvService)
        {
            _emailService = emailService;
            _csvService = csvService;
        }


        public async Task SendInvoiceAsync(InvoiceRequestDto invoice)
        {
            var sb = new StringBuilder();
            var csvBytes = _csvService.GenerateProductsCsv(invoice.Products);


            sb.Append("<h2>Your Invoice</h2>");
            sb.Append("<ul>");

            foreach (var p in invoice.Products)
            {
                sb.Append($"<li>{p.ProductName} — ${p.UnitPrice}</li>");
            }

            sb.Append("</ul>");

            await _emailService.SendEmailAsync(
                invoice.UserEmail,
                "Your Invoice",
                sb.ToString(),
                csvBytes,
                "invoice.csv"
            );
        }
    }
}
