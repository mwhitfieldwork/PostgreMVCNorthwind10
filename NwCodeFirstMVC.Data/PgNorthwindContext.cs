using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NwCodeFirstMVC.Data;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NWCodeFirstMVC.Data.PgModels
{
    public partial class PgNorthwindContext : DbContext
    {
        public PgNorthwindContext()
        {
        }

        public PgNorthwindContext(DbContextOptions<PgNorthwindContext> options)
            : base(options)
        {
        }
    }
}
