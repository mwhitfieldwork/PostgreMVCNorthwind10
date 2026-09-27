using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class AdminUserDto
    {
        public int Pkid { get; set; }
        public string? Username { get; set; }
        public bool Admin { get; set; }
        public string? Firstname { get; set; }
        public string? Occupation { get; set; }
    }
}
