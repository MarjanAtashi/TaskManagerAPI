using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.TaskItemsDTO;
using TaskManagerAPI.Services.Interfaces;
namespace TaskManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemsService _taskItemsService;
    public TaskItemsController(ITaskItemsService taskItemsService)
    {
        _taskItemsService = taskItemsService;
    }

    // list all tasks in basic version---------------------------------------------------------------------------------------
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemsResponseDto>>> GetAllTasks()
    {
        var result = await _taskItemsService.GetAllTasks();

        return Ok(result);
    }

    // list all tasks grouped by project-------------------------------------------------------------------------------------
    [HttpGet("grouped")]
    public async Task<ActionResult<IEnumerable<TasksGroupedByProjectDto>>> GetAllGroupedTasks()
    {
        var result = await _taskItemsService.GetAllGroupedTasks();

        return Ok(result);
    }

    // get a task item by id-------------------------------------------------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemsResponseDto>> GetTaskItem(int id)
    {
        var result = await _taskItemsService.GetTaskItem(id);

        return Ok(result);
    }

    // Get all tasks for a specific project----------------------------------------------------------------------------------
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskItemsResponseDto>>> GetTasksByProject(int projectId)
    {
        var result = await _taskItemsService.GetTasksByProject(projectId);
        return Ok(result);
    }

    // create a new task item------------------------------------------------------------------------------------------------
    [HttpPost]
    public async Task<ActionResult<TaskItemsResponseDto>> CreateTask(TaskCreateDto newTaskItem)
    {
        var result = await _taskItemsService.CreateTask(newTaskItem);

        return CreatedAtAction(nameof(GetTaskItem), new { id = result.Id }, result);
    }

    // update a task item by id----------------------------------------------------------------------------------------------
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto updatedTask)
    {
        await _taskItemsService.UpdateTask(id, updatedTask);

        return Ok("Task updated successfully.");
    }

    // Delete a task item by id----------------------------------------------------------------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItem(int id)
    {
        await _taskItemsService.DeleteTask(id);

        return Ok("Task deleted sucessfuly.");
    }
}
