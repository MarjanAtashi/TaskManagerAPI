namespace TaskManagerAPI.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

    }
}
