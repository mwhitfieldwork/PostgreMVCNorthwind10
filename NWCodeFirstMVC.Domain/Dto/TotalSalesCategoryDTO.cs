using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class TotalSalesCategoryDTO
    {
        public float SaleAmount { get; set; }
        public string Country { get; set; }
        public int OrderId { get; set; }    
    }
}
