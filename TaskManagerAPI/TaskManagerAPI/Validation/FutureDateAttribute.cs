using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Validation
{
    public class FutureDateAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is DateTime date && date < DateTime.UtcNow)
            {
                return new ValidationResult("DeadLine cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}