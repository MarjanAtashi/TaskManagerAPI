using System.Security.Claims;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(claim) || !int.TryParse(claim, out var id))
                    throw new UnauthorizedAccessException("User id not found in token.");

                return id;
            }
        }

        public string Username => _httpContextAccessor.HttpContext?
            .User.FindFirst(ClaimTypes.Name)?.Value ?? throw new UnauthorizedAccessException("Username not found in token.");

        public string Role => _httpContextAccessor.HttpContext?
            .User.FindFirst(ClaimTypes.Role)?.Value ?? throw new UnauthorizedAccessException("User role not found in token.");
    }
}
