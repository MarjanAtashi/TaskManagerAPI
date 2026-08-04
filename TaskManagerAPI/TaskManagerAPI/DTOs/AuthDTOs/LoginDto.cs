using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs.AuthDTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is require")]
        public string UsernameOrEmail { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
