using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.AuthDTOs;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
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

        // register user ---------------------------------------------------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }

        // Refresh user ---------------------------------------------------------------------------------------------------
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshAsync(request);
            return Ok(result);
        }

        // Logout user ----------------------------------------------------------------------------------------------------
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto request)
        {
            await _authService.LogoutAsync(request);
            return Ok("Logout successful.");
        }
    }
}
