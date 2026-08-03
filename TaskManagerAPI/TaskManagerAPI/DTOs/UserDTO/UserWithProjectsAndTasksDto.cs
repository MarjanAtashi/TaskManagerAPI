using TaskManagerAPI.DTOs.SharedDtos;

namespace TaskManagerAPI.DTOs.UserDTO
{
    public class UserWithProjectsAndTasksDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        public List<FullProjectSummaryDto> Projects { get; set; } = new();

    }
}
