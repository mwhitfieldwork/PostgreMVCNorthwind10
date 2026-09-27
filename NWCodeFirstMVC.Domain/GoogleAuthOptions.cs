using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain
{
    public class GoogleAuthOptions
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string RedirectUri { get; set; } = default!;
    }
}
