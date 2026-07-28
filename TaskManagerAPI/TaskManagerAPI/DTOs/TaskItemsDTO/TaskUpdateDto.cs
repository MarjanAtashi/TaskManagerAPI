namespace TaskManagerAPI.DTOs.TaskItemsDTO
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? DeadLine { get; set; }

    }
}
