using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class CustomerOrderDTO
    {
        [DataMember]
        public string CustomerID { get; set; } = null!;
        [DataMember]
        public string ProductName { get; set; } = null!;

        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string CategoryName { get; set; } = null!;

        [DataMember]
        public float UnitPrice { get; set; }

        [DataMember]
        public decimal Quantity { get; set; }

        [DataMember]
        public System.DateOnly? OrderDate { get; set; } = null!;
    }
}
