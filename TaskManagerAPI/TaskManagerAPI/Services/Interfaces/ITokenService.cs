using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken();
    }
}
