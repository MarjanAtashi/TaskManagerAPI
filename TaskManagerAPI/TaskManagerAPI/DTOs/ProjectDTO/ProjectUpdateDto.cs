namespace TaskManagerAPI.DTOs.ProjectDTO
{
    public class ProjectUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }

        public int UserId { get; set; }


    }
}
