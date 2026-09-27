using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure.Services;
using static NWCodeFirstMVC.Domain.GoogleAuthModels;

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IMapper mapper;
        private readonly IGenericRepository<NWCodeFirstMVC.Infrastructure.PgModels.User> _service;
        private readonly IGoogleAuthService _googleAuthService;

        public LoginController(
            ILoginService loginService, 
            IMapper mapper, 
            IGenericRepository<NWCodeFirstMVC.Infrastructure.PgModels.User> service,
            IGoogleAuthService googleAuthService
            )
        {
            _loginService = loginService;
            this.mapper = mapper;
            _service = service;
            _googleAuthService = googleAuthService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var user = await _loginService.GetAllAsync();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(User userModel)
        {
            try
            {
                var user = await _loginService.Authenticate(userModel);
                if (user == null) return Unauthorized("Invalid credentials.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddProduct(User createUser)
        {
            var user = mapper.Map<User>(createUser);
            var results = await _loginService.AddAsync(user);
            return Ok(user);
        }

        [HttpPost("GoogleCallback")]
        public async Task<IActionResult> GoogleCallback([FromBody] GoogleTokenExchangeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return BadRequest(new { message = "Missing authorization code." });

            try
            {
                var tokenResponse = await _googleAuthService.ExchangeCodeAsync(request.Code);
                var googleUser = await _googleAuthService.GetUserInfoAsync(tokenResponse.AccessToken);

                return await _loginService.AuthenticateWithGoogle(googleUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
