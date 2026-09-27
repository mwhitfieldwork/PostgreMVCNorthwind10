using System;
using System.Collections.Generic;

namespace NWCodeFirstMVC.Domain.PocoModels
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<ProductModel>();
        }

        public int SupplierId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? HomePage { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
    }
}
