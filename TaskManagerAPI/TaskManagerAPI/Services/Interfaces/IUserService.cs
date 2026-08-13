using TaskManagerAPI.DTOs.UserDTO;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsers();

        Task<UserResponseDto> GetUser(int id);

        Task<UserWithProjectsDto> GetUserByIdWithDetails(int id);

        Task<UserWithProjectsAndTasksDto> GetUserByIdWithProjectsAndTasks(int id);

        Task UpdateUser(int id, UserUpdateDto updatedUserDto);

        Task DeleteUser(int id);

    }
}
