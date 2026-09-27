using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeTerritoryMapController : ControllerBase
    {
        private readonly IEmployeeTerritoryMap _service;

        public EmployeeTerritoryMapController(IEmployeeTerritoryMap service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeTerritoryMapDto>>> Get()
        {
            var result = await _service.GetEmployeeTerritoryMap();
            return Ok(result);
        }
    }
}
