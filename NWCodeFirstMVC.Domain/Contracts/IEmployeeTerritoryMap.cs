using NWCodeFirstMVC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IEmployeeTerritoryMap : IGenericRepository<EmployeeTerritoryMapDto>
    {
        Task<List<EmployeeTerritoryMapDto>> GetEmployeeTerritoryMap();
    }
}
