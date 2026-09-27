using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;

using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET: DashboardController
        private readonly IEmployeeService _employeeService;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this._employeeService = employeeService;
            this.mapper = mapper;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeController>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeDTO createemp)
        {
            var employee = mapper.Map<Employee>(createemp);
            var results = await _employeeService.AddAsync(employee);
            return CreatedAtAction("GetAllEmployees", new { EmployeeId = employee.EmployeeId }, employee);
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee (int id)
        {
            await _employeeService.DeleteAsync(id);

            return NoContent();

        }
    }
}
