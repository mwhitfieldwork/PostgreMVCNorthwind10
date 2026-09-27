using System.Collections.Generic;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class InvoiceRequestDto
    {
        public string UserEmail { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
