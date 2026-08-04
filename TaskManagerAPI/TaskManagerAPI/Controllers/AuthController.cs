using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.AuthDTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;

        public AuthController(AppDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                return BadRequest("Username already exists.");

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return BadRequest("Email already exists.");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, new RegisterResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                      .FirstOrDefaultAsync(u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var isPasswordValid = _passwordService.VerifyPassword(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new LoginResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }

    }
}
