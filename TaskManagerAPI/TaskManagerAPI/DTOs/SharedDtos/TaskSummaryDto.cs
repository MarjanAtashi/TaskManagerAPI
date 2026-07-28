namespace TaskManagerAPI.DTOs.SharedDtos
{
    public class TaskSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? DeadLine { get; set; }
    }
}
