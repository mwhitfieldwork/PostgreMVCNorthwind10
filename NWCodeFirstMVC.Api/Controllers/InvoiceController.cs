using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public record InvoiceIdsRequest(List<int> ProductIds);

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceIdsRequest request)
        {
            var email = User.FindFirst("email")?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("No email found in your login token.");

            if (request.ProductIds is null || request.ProductIds.Count == 0)
                return BadRequest("No products selected.");

            try
            {
                await _invoiceService.SendInvoiceAsync(email, request.ProductIds);
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

