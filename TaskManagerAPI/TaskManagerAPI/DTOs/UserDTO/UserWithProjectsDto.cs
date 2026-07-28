using TaskManagerAPI.DTOs.SharedDtos;

namespace TaskManagerAPI.DTOs.UserDTO
{
    public class UserWithProjectsDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        public int Count { get; set; }

        public List<ProjectSummaryDto> Projects { get; set; } = new List<ProjectSummaryDto>();
    }
}
