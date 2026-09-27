using System.Text;
using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IEmailService _emailService;

        public InvoiceService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendInvoiceAsync(InvoiceRequestDto invoice)
        {
            var sb = new StringBuilder();

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
                sb.ToString()
            );
        }
    }
}
