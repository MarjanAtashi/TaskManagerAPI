namespace TaskManagerAPI.DTOs.ProjectDTO
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
