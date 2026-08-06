using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, PasswordService passwordService, TokenService tokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
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
            var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var isPasswordValid = _passwordService.VerifyPassword(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new LoginResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var refreshToken = await _context.RefreshTokens
               .Include(rt => rt.User)
               .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);


            if (refreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            if (refreshToken.Expires <= DateTime.UtcNow)
            {
                return Unauthorized("Refresh token has expired.");
            }
            if (refreshToken.Revoked != null)
            {
                return Unauthorized("Refresh token has been revoked.");
            }

            refreshToken.Revoked = DateTime.UtcNow;

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            newRefreshToken.UserId = refreshToken.UserId;

            _context.RefreshTokens.Add(newRefreshToken);

            var newAccessToken = _tokenService.GenerateAccessToken(refreshToken.User);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            });

        }

    }
}
