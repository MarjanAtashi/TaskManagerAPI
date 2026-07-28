namespace TaskManagerAPI.DTOs.SharedDtos
{
    public class ProjectSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
