using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.DTOs.UserDTO;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        //List of all users --------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _context.Users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username
            }).ToListAsync();

            return Ok(users);
        }

        // Get a specific user by Id -----------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {

            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var user = await _context.Users
          .Select(u => new UserResponseDto
          {
              Id = u.Id,
              Username = u.Username
          }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound("There is no user with the specified ID.");


            return Ok(user);
        }

        // Get a specific user by Id with project details -------------------------------------------------------------------
        [HttpGet("{id}/projects")]
        public async Task<ActionResult<UserWithProjectsDto>> GetUserByIdWithDetails(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var user = await _context.Users
                .Select(u => new UserWithProjectsDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    ProjectCount = u.Projects.Count,
                    Projects = u.Projects.Select(p => new ProjectSummaryDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        DeadLine = p.DeadLine
                    }).ToList()
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound("There is no user with the specified ID.");


            return Ok(user);
        }

        // Get a specific user by Id with project details and its tasks -----------------------------------------------------
        [HttpGet("{id}/projects/details")]
        public async Task<ActionResult<UserWithProjectsAndTasksDto>> GetUserByIdWithProjectsAndTasks(int id)
        {

            if (id <= 0 )
                return BadRequest("Id must be a positive number.");

            var user = await _context.Users
                .Select(u => new UserWithProjectsAndTasksDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Projects = u.Projects.Select(p => new FullProjectSummaryDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        TaskItems = p.TaskItems.Select(t => new TaskSummaryDto
                        {
                            Id = t.Id,
                            Title = t.Title,
                            IsCompleted = t.IsCompleted
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound("There is no user with the specified ID.");

            return Ok(user);
        }


        // Create a new user ------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseDto);
        }

        // Update an existing user -------------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updatedUserDto)
        {

            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("There is no user with the specified ID.");


            user.Username = updatedUserDto.Username;
            user.Password = updatedUserDto.Password;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a user -----------------------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive number.");

            if (id == 1)
                return BadRequest("System user cannot be deleted.");


            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("There is no user with the specified ID.");

            var projects=await _context.Projects.Where(p => p.UserId == id).ToListAsync();

            foreach (var project in projects)
                project.UserId = 1;


            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
