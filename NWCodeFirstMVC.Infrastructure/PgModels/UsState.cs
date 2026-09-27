using System;
using System.Collections.Generic;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    public partial class UsState
    {
        public short StateId { get; set; }
        public string? StateName { get; set; }
        public string? StateAbbr { get; set; }
        public string? StateRegion { get; set; }
    }
}
