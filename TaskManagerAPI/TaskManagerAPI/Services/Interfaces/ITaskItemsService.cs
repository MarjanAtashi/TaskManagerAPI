using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.TaskItemsDTO;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITaskItemsService
    {
        Task<IEnumerable<TaskItemsResponseDto>> GetAllTasks();

        Task<IEnumerable<TasksGroupedByProjectDto>> GetAllGroupedTasks();

        Task<TaskItemsResponseDto> GetTaskItem(int id);

        Task<IEnumerable<TaskItemsResponseDto>> GetTasksByProject(int projectId);

        Task<TaskItemsResponseDto> CreateTask(TaskCreateDto newTaskItem);

        Task UpdateTask(int id, TaskUpdateDto updatedTask);

        Task DeleteTask(int id);



    }
}
