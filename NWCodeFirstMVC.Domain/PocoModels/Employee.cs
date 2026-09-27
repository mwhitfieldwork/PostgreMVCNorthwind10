using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NWCodeFirstMVC.Domain.PocoModels
{
    public partial class Employee
    {
        public Employee()
        {
            InverseReportsToNavigation = new HashSet<Employee>();
            Orders = new HashSet<Order>();
            Territories = new HashSet<Territory>();
        }

        public int EmployeeId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public byte[]? Photo { get; set; }
        public string? Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual Employee? ReportsToNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<Territory> Territories { get; set; }
    }
}
