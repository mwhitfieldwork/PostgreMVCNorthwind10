using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWCodeFirstMVC.Infrastructure.PgModels;

namespace NWCodeFirstMVC.Infrastructure.Repositories
{
    public class ProductRepository:GenericRepository<Product>
    {
        public ProductRepository(PgNwContext dc) : base(dc) { }
    }
}
