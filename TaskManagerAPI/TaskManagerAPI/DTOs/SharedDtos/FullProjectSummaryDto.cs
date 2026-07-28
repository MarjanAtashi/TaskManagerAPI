namespace TaskManagerAPI.DTOs.SharedDtos
{
    public class FullProjectSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }

        public List<TaskSummaryDto> TaskItems { get; set; } = new List<TaskSummaryDto>();
    }
}
