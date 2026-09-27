using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class EmployeeDTO
    {
        [DataMember]
        public string LastName { get; set; } = null!;
        [DataMember]
        public string FirstName { get; set; } = null!;
        [DataMember]
        public string? Title { get; set; }
        [DataMember]
        public string? TitleOfCourtesy { get; set; }
        [DataMember]
        public DateTime? BirthDate { get; set; }
        [DataMember]
        public DateTime? HireDate { get; set; }
        [DataMember]
        public string? Address { get; set; }
        [DataMember]
        public string? City { get; set; }
        [DataMember]
        public string? Region { get; set; }
        [DataMember]
        public string? PostalCode { get; set; }
        [DataMember]
        public string? Country { get; set; }
        [DataMember]
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }

    }
}
