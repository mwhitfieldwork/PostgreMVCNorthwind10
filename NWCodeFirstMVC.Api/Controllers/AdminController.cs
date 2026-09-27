using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetAllAsync();
            var dto = _mapper.Map<List<AdminUserDto>>(users);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _adminService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Pkid}, created);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _adminService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
    }
}
