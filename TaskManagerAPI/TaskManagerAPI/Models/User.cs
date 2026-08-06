namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "User";

        public List<Project> Projects { get; set; } = [];

        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
