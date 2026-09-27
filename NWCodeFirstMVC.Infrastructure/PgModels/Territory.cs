using System;
using System.Collections.Generic;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    public partial class Territory
    {
        public Territory()
        {
            Employees = new HashSet<Employee>();
        }

        public string TerritoryId { get; set; } = null!;
        public string TerritoryDescription { get; set; } = null!;
        public short RegionId { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual Region Region { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
