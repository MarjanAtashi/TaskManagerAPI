namespace TaskManagerAPI.DTOs.TaskItemsDTO
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? DeadLine { get; set; }

        public int ProjectId { get; set; }

    }
}
