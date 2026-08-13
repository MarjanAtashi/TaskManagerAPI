using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.AuthDTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly PasswordService _passwordService;

        public AuthService(AppDbContext context, ITokenService tokenService, PasswordService passwordService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }
        // register user ---------------------------------------------------------------------------------------------------
        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _context.Users.Where(u => u.Username == registerDto.Username || u.Email == registerDto.Email)
                .FirstOrDefaultAsync();

            if (existingUser?.Username == registerDto.Username)
            {
                throw new InvalidOperationException("Username already exists.");
            }
            if (existingUser?.Email == registerDto.Email)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
            };
        }
        // Login user ---------------------------------------------------------------------------------------------------
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username/email or password.");
            }

            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid username/email or password.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
        // Refresh user ---------------------------------------------------------------------------------------------------
        public async Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenRequestDto request)
        {
            var refreshToken = await
            _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);


            if (refreshToken == null)
            {
                throw new InvalidOperationException("Invalid refresh token.");
            }
            if (refreshToken.Expires <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Refresh token has expired.");
            }
            if (refreshToken.Revoked != null)
            {
                throw new InvalidOperationException("Refresh token has been revoked.");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            var newAccessToken = _tokenService.GenerateAccessToken(refreshToken.User);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            newRefreshToken.UserId = refreshToken.UserId;

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new RefreshTokenResponseDto
            {
                RefreshToken = newRefreshToken.Token,
                AccessToken = newAccessToken

            };
        }
        // Logout user ----------------------------------------------------------------------------------------------------
        public async Task LogoutAsync(RefreshTokenRequestDto request)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (refreshToken == null)
                throw new InvalidOperationException("Invalid refresh token.");

            if (refreshToken.Revoked != null)
                throw new InvalidOperationException("Refresh token has been revoked.");

            if (refreshToken.Expires < DateTime.UtcNow)
                throw new InvalidOperationException("Refresh token has expired.");

            refreshToken.Revoked = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}
