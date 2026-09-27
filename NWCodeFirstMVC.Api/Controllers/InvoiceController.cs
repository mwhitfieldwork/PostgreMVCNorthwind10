using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequestDto invoice)
        {
            try
            {
                await _invoiceService.SendInvoiceAsync(invoice);
                return Ok(new { message = "Invoice sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("INVOICE ERROR:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail([FromServices] IEmailService email)
        {
            await email.SendEmailAsync("your-email@example.com", "Test", "<h1>Brevo works!</h1>");
            return Ok("Sent");
        }
    }
}

