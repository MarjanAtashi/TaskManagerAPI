using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Validation;

namespace TaskManagerAPI.DTOs.UserDTO
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [UsernameValidationAttribute]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [PasswordValidationAttribute]
        public string Password { get; set; } = null!;
    }
}
