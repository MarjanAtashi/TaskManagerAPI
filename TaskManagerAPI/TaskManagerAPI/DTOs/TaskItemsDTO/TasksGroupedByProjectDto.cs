using TaskManagerAPI.DTOs.SharedDtos;

namespace TaskManagerAPI.DTOs.TaskItemsDTO
{
    public class TasksGroupedByProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public List<TaskSummaryDto> Tasks { get; set; } = new List<TaskSummaryDto>();
    }
}
