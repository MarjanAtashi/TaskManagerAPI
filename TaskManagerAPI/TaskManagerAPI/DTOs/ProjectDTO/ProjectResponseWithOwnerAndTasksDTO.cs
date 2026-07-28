using TaskManagerAPI.DTOs.SharedDtos;

namespace TaskManagerAPI.DTOs.ProjectDTO
{
    public class ProjectResponseWithOwnerAndTasksDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }= null!;

        public List<TaskSummaryDto> Tasks { get; set; }= new List<TaskSummaryDto>();
    }
}
