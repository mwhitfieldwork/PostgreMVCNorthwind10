using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public  class EmployeeService : GenericService<Employee>, IEmployeeService
    {
        public EmployeeService(PgNwContext dc) : base(dc)
        {
        }
    }
}
