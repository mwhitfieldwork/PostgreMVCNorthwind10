using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class TerritoryMapDto
    {
        public string TerritoryId { get; set; } = null!;
        public string TerritoryDescription { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public short RegionId { get; set; }
        public string RegionDescription { get; set; } = null!;
    }
}
