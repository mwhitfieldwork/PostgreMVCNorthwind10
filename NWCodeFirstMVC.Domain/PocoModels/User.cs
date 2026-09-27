using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.PocoModels
{
    public class User
    {
        public int PKID { get; set; }
        public string? UserName { get; set; }
        public string? Passowrd { get; set; }

        public bool admin { get; set; }

        public string? firstName { get; set; }
        public string? occupation { get; set; }
    }
}
