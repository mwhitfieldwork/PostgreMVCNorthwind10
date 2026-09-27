using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Dto
{
    public class EmployeeTerritoryMapDto
    {
        public short? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? PhotoPath { get; set; }
        public List<TerritoryMapDto> Territories { get; set; } = new();
    }
}
