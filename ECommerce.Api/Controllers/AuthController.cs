using ECommerce.BLL.DTOs;
using ECommerce.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == null) return BadRequest(new { message = "User already exists or invalid data" });
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null) return Unauthorized();
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (result == null) return Unauthorized();
            return Ok(result);
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return BadRequest();
            var ok = await _authService.RevokeRefreshTokenAsync(username);
            if (!ok) return BadRequest();
            return NoContent();
        }
    }
}
