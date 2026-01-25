using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Auth.Services;
using ProjetoEcommerce.Application.Auth.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
