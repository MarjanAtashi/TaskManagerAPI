using TaskManagerAPI.DTOs.ProjectDTO;
using TaskManagerAPI.DTOs.SharedDtos;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectSummaryDto>> GetAllProjects();

        Task<FullProjectSummaryDto> GetProject(int id);

        Task<ProjectResponseDto> CreateProject(ProjectCreateDto newProject);

        Task UpdateProject(int id, ProjectUpdateDto updatedProject);

        Task DeleteProject(int id);
    }
}
