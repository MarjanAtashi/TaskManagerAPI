using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs.UserDTO
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100,MinimumLength =3,ErrorMessage = "Username must be between 3 and 100 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; } = null!;
    }
}
