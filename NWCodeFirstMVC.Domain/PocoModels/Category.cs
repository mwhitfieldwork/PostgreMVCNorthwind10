using System;
using System.Collections.Generic;

namespace NWCodeFirstMVC.Domain.PocoModels
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<ProductModel>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
    }
}
