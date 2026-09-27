using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class GetProductDto
    {
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public Nullable<int> SupplierID { get; set; }
        [DataMember]
        public Nullable<int> CategoryID { get; set; }
        [DataMember]
        public string QuantityPerUnit { get; set; }
        [DataMember]
        public Nullable<decimal> UnitPrice { get; set; }
        [DataMember]
        public Nullable<short> UnitsInStock { get; set; }
        [DataMember]
        public Nullable<short> UnitsOnOrder { get; set; }
        [DataMember]
        public Nullable<short> ReorderLevel { get; set; }
        [DataMember]
        public Nullable<bool> Discontinued { get; set; }
        [DataMember]
        public Nullable<bool> IsDeleted { get; set; }
    }
}
