using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.DTOs.TaskItemsDTO;
using TaskManagerAPI.DTOs.UserDTO;
using TaskManagerAPI.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    public TaskItemsController(AppDbContext context)
    {
        _context = context;
    }

    // list all tasks in basic version---------------------------------------------------------------------------------------
    [HttpGet("taskitems/flat")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TaskItemsResponseDto>>> GetAllTasks()
    {
        var taskItems = await _context.TaskItems.Select(t => new TaskItemsResponseDto
        {
            Id = t.Id,
            Title = t.Title,
            DeadLine = t.DeadLine,
            IsCompleted = t.IsCompleted,
            ProjectId = t.ProjectId,
            ProjectName = t.Project!.Title
        }).ToListAsync();

        return Ok(taskItems);
    }

    // list all tasks grouped by project-------------------------------------------------------------------------------------
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TasksGroupedByProjectDto>>> GetTasksByProjects()
    {
        var taskItems = await _context.TaskItems.GroupBy(t => t.ProjectId)
       .Select(t => new TasksGroupedByProjectDto
       {
           ProjectId = t.Key,
           ProjectTitle = t.First().Project!.Title,
           TaskItems = t.Select(task => new TaskSummaryDto
           {
               Id = task.Id,
               Title = task.Title,
               IsCompleted = task.IsCompleted,
               DeadLine = task.DeadLine
           }).ToList()
       }).ToListAsync();

        return Ok(taskItems);
    }

    // get a task item by id-------------------------------------------------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemsResponseDto>> GetTaskItem(int id)
    {
        if (id <= 0)
            return BadRequest("Id must be a positive number.");

        var taskItem = await _context.TaskItems
            .Select(t => new TaskItemsResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                DeadLine = t.DeadLine,
                IsCompleted = t.IsCompleted,
                ProjectId = t.ProjectId,
                ProjectName = t.Project!.Title
            }).FirstOrDefaultAsync(x => x.Id == id);

        if (taskItem == null)
            return NotFound("The specified TaskItem does not exist.");

        return Ok(taskItem);
    }

    // create a new task item------------------------------------------------------------------------------------------------
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TaskItemsResponseDto>> PostTaskItem(TaskCreateDto newTaskItem)
    {
        var project = await _context.Projects
            .Where(p => p.Id == newTaskItem.ProjectId)
            .Select(p => new { p.Id, p.Title })
            .FirstOrDefaultAsync();

        if (project == null)
            return BadRequest("The specified ProjectId does not exist.");

        var taskItem = new TaskItem
        {
            Title = newTaskItem.Title,
            IsCompleted = newTaskItem.IsCompleted,
            DeadLine = newTaskItem.DeadLine,
            ProjectId = newTaskItem.ProjectId
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        var responseDto = new TaskItemsResponseDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            IsCompleted = taskItem.IsCompleted,
            DeadLine = taskItem.DeadLine,
            ProjectId = taskItem.ProjectId,
            ProjectName = project.Title
        };

        return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, responseDto);
    }
    // update a task item by id----------------------------------------------------------------------------------------------
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto updatedTask)
    {
        if (id <= 0)
            return BadRequest("Id must be a positive number.");

        var task = await _context.TaskItems.FindAsync(id);

        if (task == null)
            return NotFound("The specified TaskItem does not exist.");

        task.Title = updatedTask.Title;
        task.IsCompleted = updatedTask.IsCompleted;
        task.DeadLine = updatedTask.DeadLine;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Delete a task item by id----------------------------------------------------------------------------------------------
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTaskItem(int id)
    {
        if (id <= 0)
            return BadRequest("Id must be a positive number.");

        var taskitem = await _context.TaskItems.FindAsync(id);

        if (taskitem == null)
            return NotFound("The specified TaskItem does not exist.");

        _context.TaskItems.Remove(taskitem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Get all tasks for a specific project----------------------------------------------------------------------------------
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskItemsResponseDto>>> GetTasksByProject(int projectId)
    {
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);

        if (!projectExists)
            return NotFound("The specified Project does not exist.");

        var tasks = await _context.TaskItems
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TaskItemsResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                DeadLine = t.DeadLine,
                ProjectId = t.ProjectId,
                ProjectName = t.Project!.Title
            })
            .ToListAsync();

        return Ok(tasks);
    }
}
