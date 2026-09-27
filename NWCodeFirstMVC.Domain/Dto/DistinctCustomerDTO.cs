using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class DistinctCustomerDTO
    {
        [DataMember]
        public string? ContactName { get; set; } = null!;

        [DataMember]
        public string CustomerID { get; set; } = null!;
    }
}
