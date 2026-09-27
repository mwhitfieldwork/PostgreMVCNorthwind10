using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class EmployeeTerritoryMapService : GenericService<EmployeeTerritoryMapDto>, IEmployeeTerritoryMap
    {
        private readonly PgNwContext _dc;
        public EmployeeTerritoryMapService(PgNwContext dc) : base(dc)
        {
            this._dc = dc;
        }

        public async Task<List<EmployeeTerritoryMapDto>> GetEmployeeTerritoryMap()
        {
            var employees = await _dc.Employees
                .Select(e => new EmployeeTerritoryMapDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Title = e.Title,
                    PhotoPath = e.PhotoPath,
                    Territories = e.Territories.Select(t => new TerritoryMapDto
                    {
                        TerritoryId = t.TerritoryId,
                        TerritoryDescription = t.TerritoryDescription,
                        Latitude = t.Latitude,
                        Longitude = t.Longitude,
                        RegionId = t.RegionId,
                        RegionDescription = t.Region.RegionDescription
                    }).ToList()
                })
                .ToListAsync();

            var unassigned = await _dc.Territories
                .Where(t => !t.Employees.Any())
                .Select(t => new TerritoryMapDto
                {
                    TerritoryId = t.TerritoryId,
                    TerritoryDescription = t.TerritoryDescription,
                    Latitude = t.Latitude,
                    Longitude = t.Longitude,
                    RegionId = t.RegionId,
                    RegionDescription = t.Region.RegionDescription
                })
                .ToListAsync();

            if (unassigned.Count > 0)
            {
                employees.Add(new EmployeeTerritoryMapDto
                {
                    LastName = "Unassigned",
                    Territories = unassigned
                });
            }

            return employees;
        }
    }
}
