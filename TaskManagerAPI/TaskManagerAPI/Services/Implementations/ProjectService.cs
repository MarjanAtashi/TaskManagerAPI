using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.ProjectDTO;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ProjectService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // List all projects ------------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<ProjectSummaryDto>> GetAllProjects()
        {
            var query = _context.Projects.AsNoTracking();

            if (_currentUser.Role != "Admin")
                query = query.Where(t => t.UserId == _currentUser.UserId);

            return await query.Select(p => new ProjectSummaryDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DeadLine = p.DeadLine,
            }).ToListAsync();
        }

        // Get a specific project---------------------------------------------------------------------------------------
        public async Task<FullProjectSummaryDto> GetProject(int id)
        {
            if (id <= 0)
                throw new Exception("Id must be more than 0.");

            var query = _context.Projects.AsNoTracking();

            if (_currentUser.Role != "Admin")
                query = query.Where(p => p.UserId == _currentUser.UserId);

            var project = await query.Select(p => new FullProjectSummaryDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DeadLine = p.DeadLine,
                TaskItems = p.TaskItems.Select(t => new TaskSummaryDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DeadLine = t.DeadLine,
                    IsCompleted = t.IsCompleted
                }).ToList(),
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
                throw new Exception("There is no project with the specified ID.");

            return project;
        }

        // Create a new project----------------------------------------------------------------------------------------------
        public async Task<ProjectResponseDto> CreateProject(ProjectCreateDto newProject)
        {
            var project = new Project
            {
                Title = newProject.Title,
                Description = newProject.Description,
                DeadLine = newProject.DeadLine,
                UserId = _currentUser.UserId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                DeadLine = project.DeadLine,
            };
        }

        // Update an existing project----------------------------------------------------------------------------------------
        public async Task UpdateProject(int id, ProjectUpdateDto updatedProject)
        {
            if (id <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.Projects.AsQueryable();

            if (_currentUser.Role != "Admin")
                query = query.Where(p => p.UserId == _currentUser.UserId);

            var project = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
                throw new Exception("There is no project with the specified ID.");

            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            project.DeadLine = updatedProject.DeadLine;

            await _context.SaveChangesAsync();
        }

        // Delete a project--------------------------------------------------------------------------------------------------
        public async Task DeleteProject(int id)
        {
            if (id <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.Projects.AsQueryable();

            if (_currentUser.Role != "Admin")
                query = query.Where(p => p.UserId == _currentUser.UserId);

            var project = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
                throw new Exception("There is no project with the specified ID.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

        }
    }
}
