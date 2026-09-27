using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class UserDTO
    {
        [DataMember]
        public int Pkid { get; set; }
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public bool Admin { get; set; }

        [DataMember]
        public string Firstname { get; set; }

        [DataMember]
        public string Occupation { get; set; }
    }
}
