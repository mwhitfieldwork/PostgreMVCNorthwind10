using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class GetSalesDto
    {
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public string CategoryName { get; set; } = null!;
        [DataMember]
        public string? ProductName { get; set; }
        [DataMember]
        public int ProductSales { get; set; }
    }
}
