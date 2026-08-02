using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Validation;

namespace TaskManagerAPI.DTOs.ProjectDTO
{
    public class ProjectUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = " Project Title must be between 3 and 100 characters long")]
        public string Title { get; set; } = null!;

        [MaxLength(500, ErrorMessage = " Project description can be up to 500 characters long")]
        public string? Description { get; set; }

        [FutureDate(ErrorMessage = "DeadLine cannot be in the past")]
        public DateTime? DeadLine { get; set; }
    }
}
