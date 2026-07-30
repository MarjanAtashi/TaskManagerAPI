using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Validation;

namespace TaskManagerAPI.DTOs.TaskItemsDTO
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = " Task Title must be between 3 and 100 characters long")]
        public string Title { get; set; } = null!;

        public bool IsCompleted { get; set; } = false;

        [FutureDate(ErrorMessage = "DeadLine cannot be in the past")]
        public DateTime? DeadLine { get; set; }

    }
}
