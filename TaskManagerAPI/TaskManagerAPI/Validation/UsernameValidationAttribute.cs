using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaskManagerAPI.Validation
{
    public class UsernameValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string username)
            {
                return new ValidationResult("Invalid username format.");
            }

            if (username.Length < 3 || username.Length > 50)
            {
                return new ValidationResult("username must be between 3 and 50 characters long.");

            }

            var regex = new Regex(@"^[a-zA-Z0-9_]+$");

            if (!regex.IsMatch(username))
            {
                return new ValidationResult("Username can only contain letters, numbers, and underscores.");
            }


            return ValidationResult.Success;

        }
    }
}
