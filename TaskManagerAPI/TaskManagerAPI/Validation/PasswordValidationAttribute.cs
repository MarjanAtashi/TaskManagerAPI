using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaskManagerAPI.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string password)
            {
                return new ValidationResult("Invalid password format.");
            }

            if (password.Length < 8 || password.Length > 100)
            {
                return new ValidationResult("Password must be between 8 and 100 characters long.");
            }

            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$");


            if (!regex.IsMatch(password))
            {
                return new ValidationResult("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
            }


            return ValidationResult.Success;
        }
    }
}
