using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.ProjectDTO;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // List all projects ------------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectsWithOwnerDto>>> GetAllProjects()
        {
            var result = await _projectService.GetAllProjects();

            return Ok(result);
        }

        // Get a specific project -------------------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseWithOwnerAndTasksDTO>> GetProject(int id)
        {
            var result = await _projectService.GetProject(id);

            return Ok(result);
        }

        // Create a new project----------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject(ProjectCreateDto newProject)
        {
            var result = await _projectService.CreateProject(newProject);

            return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
        }

        // Update an existing project----------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto updatedProject)
        {
            await _projectService.UpdateProject(id, updatedProject);

            return Ok("Project updated sucessfuly.");
        }

        // Delete a project--------------------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProject(id);

            return Ok("Project deleted sucessfuly.");
        }

    }

}

