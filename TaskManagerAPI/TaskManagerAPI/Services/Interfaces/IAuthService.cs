using TaskManagerAPI.DTOs.AuthDTOs;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);

        Task<RefreshTokenResponseDto> RefreshAsync(RefreshTokenRequestDto request);

        Task LogoutAsync(RefreshTokenRequestDto request);

    }
}
