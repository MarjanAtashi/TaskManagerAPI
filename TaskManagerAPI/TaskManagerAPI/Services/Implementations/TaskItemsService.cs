using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.DTOs.TaskItemsDTO;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Implementations
{
    public class TaskItemsService : ITaskItemsService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public TaskItemsService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        // list all tasks in basic version---------------------------------------------------------------------------------------
        public async Task<IEnumerable<TaskItemsResponseDto>> GetAllTasks()
        {
            var query = _context.TaskItems.AsNoTracking();

            if (_currentUser.Role != "Admin")
            {
                query = query.Where(t => t.Project!.UserId == _currentUser.UserId);
            }

            return await query.Select(t => new TaskItemsResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                DeadLine = t.DeadLine,
                ProjectId = t.ProjectId,
                ProjectName = t.Project!.Title
            }).ToListAsync();
        }

        // list all tasks grouped by project-------------------------------------------------------------------------------------
        public async Task<IEnumerable<TasksGroupedByProjectDto>> GetAllGroupedTasks()
        {
            var query = _context.Projects.AsNoTracking();

            if (_currentUser.Role != "Admin")
                query = query.Where(p => p.UserId == _currentUser.UserId);

            return await query.Select(p => new TasksGroupedByProjectDto
            {
                ProjectId = p.Id,
                ProjectTitle = p.Title,
                TaskItems = p.TaskItems.Select(t => new TaskSummaryDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DeadLine = t.DeadLine,
                    IsCompleted = t.IsCompleted
                }).ToList()
            }).ToListAsync();
        }

        // get a task item by id-------------------------------------------------------------------------------------------------
        public async Task<TaskItemsResponseDto> GetTaskItem(int id)
        {
            if (id <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.TaskItems.AsNoTracking();

            if (_currentUser.Role != "Admin")
                query = query.Where(t => t.Project!.UserId == _currentUser.UserId);


            var task = await query.Select(t => new TaskItemsResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                DeadLine = t.DeadLine,
                IsCompleted = t.IsCompleted,
                ProjectId = t.ProjectId,
                ProjectName = t.Project!.Title
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("There is no sepesific task with this Id.");

            return task;
        }

        // Get all tasks for a specific project----------------------------------------------------------------------------------
        public async Task<IEnumerable<TaskItemsResponseDto>> GetTasksByProject(int projectId)
        {
            if (projectId <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.TaskItems.AsNoTracking().Where(t => t.ProjectId == projectId);

            if (_currentUser.Role != "Admin")
                query = query.Where(t => t.Project!.UserId == _currentUser.UserId);

            var tasks = await query.Select(t => new TaskItemsResponseDto
            {
                ProjectId = t.ProjectId,
                ProjectName = t.Project!.Title,
                Id = t.Id,
                Title = t.Title,
                DeadLine = t.DeadLine,
                IsCompleted = t.IsCompleted,
            }).ToListAsync();

            return tasks;
        }

        // create a new task item------------------------------------------------------------------------------------------------
        public async Task<TaskItemsResponseDto> CreateTask(TaskCreateDto newTaskItem)
        {
            if (newTaskItem == null)
                throw new Exception("Task is null.");

            var query = _context.Projects.AsQueryable();

            if (_currentUser.Role != "Admin")
            {
                query = query.Where(p => p.UserId == _currentUser.UserId);
            }

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == newTaskItem.ProjectId);

            if (project == null)
                throw new Exception("There is no project with the specified ID.");

            var task = new TaskItem
            {
                Title = newTaskItem.Title,
                DeadLine = newTaskItem.DeadLine,
                IsCompleted = newTaskItem.IsCompleted,
                ProjectId = newTaskItem.ProjectId,
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return new TaskItemsResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                DeadLine = task.DeadLine,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                ProjectName = project.Title
            };

        }

        // update a task item by id----------------------------------------------------------------------------------------------
        public async Task UpdateTask(int id, TaskUpdateDto updatedTask)
        {
            if (id <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.TaskItems.AsQueryable();

            if (_currentUser.Role != "Admin")
                query = query.Where(t => t.Project!.UserId == _currentUser.UserId);

            var task = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("There is no task with the specified ID.");

            task.Title = updatedTask.Title;
            task.DeadLine = updatedTask.DeadLine;
            task.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();
        }

        // Delete a task item by id----------------------------------------------------------------------------------------------
        public async Task DeleteTask(int id)
        {
            if (id <= 0)
                throw new Exception("Id must be a positive number.");

            var query = _context.TaskItems.AsQueryable();

            if (_currentUser.Role != "Admin")
                query = query.Where(t => t.Project!.UserId == _currentUser.UserId);

            var task = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("There is no task with the specified ID.");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

        }
    }
}
