using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.ProjectDTO;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // List all projects with owner--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectsWithOwnerDto>>> GetAllProjects()
        {
            var projects = await _context.Projects
            .Select(p => new ProjectsWithOwnerDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                UserId = p.UserId,
                DeadLine = p.DeadLine,
                Username = p.User!.Username
            }).ToListAsync();

            return Ok(projects);
        }

        // Get a specific project by ID with its owner and tasks--------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseWithOwnerAndTasksDTO>> GetProject(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var project = await _context.Projects.Select(p => new ProjectResponseWithOwnerAndTasksDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DeadLine = p.DeadLine,
                UserId = p.UserId,
                Username = p.User!.Username,
                Tasks = p.Items.Select(t => new TaskSummaryDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DeadLine = t.DeadLine,
                    IsCompleted = t.IsCompleted
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);


            if (project == null)
                return NotFound("There is no project with the specified ID.");

            return Ok(project);
        }

        // Create a new project----------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject(ProjectCreateDto newProject)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == newProject.UserId);
            if (!userExists)
                return BadRequest("The specified UserId does not exist.");

            var project = new Project()
            {
                Title = newProject.Title,
                Description = newProject.Description,
                DeadLine = newProject.DeadLine,
                UserId = newProject.UserId,
            };

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            var projectResponseDto = new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                DeadLine = project.DeadLine,
            };

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectResponseDto);
        }
        // Update an existing project----------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto updatedProject)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == updatedProject.UserId);
            if (!userExists)
                return BadRequest("The specified UserId does not exist.");

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            project.DeadLine = updatedProject.DeadLine;
            project.UserId = updatedProject.UserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a project--------------------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

