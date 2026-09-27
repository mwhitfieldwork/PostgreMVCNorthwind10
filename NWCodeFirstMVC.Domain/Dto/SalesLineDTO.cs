using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class SalesLineDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CustomerCountry { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string ShipperName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
    }
}
