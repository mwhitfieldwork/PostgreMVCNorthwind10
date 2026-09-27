using System;
using System.Collections.Generic;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        public short RegionId { get; set; }
        public string RegionDescription { get; set; } = null!;

        public virtual ICollection<Territory> Territories { get; set; }
    }
}
