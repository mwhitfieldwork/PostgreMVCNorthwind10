using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Infrastructure.PgModels
{
    public class User
    {
        public int Pkid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public string Firstname { get; set; }
        public string? Occupation { get; set; }
        public string? Picture { get; set; }
    }
}
