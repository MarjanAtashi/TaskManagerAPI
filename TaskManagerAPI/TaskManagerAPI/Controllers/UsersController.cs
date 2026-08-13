using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.UserDTO;
using TaskManagerAPI.Services.Interfaces;
namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;

        public UsersController(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        //List of all users --------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }

        // Get a specific user by Id -----------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var result = await _userService.GetUser(id);
            return Ok(result);
        }

        // Get a specific user by Id with project details -------------------------------------------------------------------
        [HttpGet("{id}/projects")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserWithProjectsDto>> GetUserByIdWithDetails(int id)
        {
            var result = await _userService.GetUserByIdWithDetails(id);
            return Ok(result);
        }

        // Get a specific user by Id with project details and its tasks -----------------------------------------------------
        [HttpGet("{id}/projects/details")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserWithProjectsAndTasksDto>> GetUserByIdWithProjectsAndTasks(int id)
        {
            var result = await _userService.GetUserByIdWithProjectsAndTasks(id);

            return Ok(result);
        }

        // Update an existing user -------------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updatedUserDto)
        {
            await _userService.UpdateUser(id, updatedUserDto);

            return Ok("User updated sucessfuly.");

        }

        // Delete a user -----------------------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok("User deleted sucessfuly.");
        }

        //-------------------------------------------------------------------------------------------------------------------
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                _currentUser.UserId,
                _currentUser.Username,
                _currentUser.Role
            });

        }
        // Create a new user ------------------------------------------------------------------------------------------------
        /*  [HttpPost]
          public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
          {
              if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                  return BadRequest("Username already exists.");

              if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                  return BadRequest("Email already exists.");

              var user = new User
              {
                  Username = dto.Username,
                  Email = dto.Email,
                  PasswordHash = _passwordService.HashPassword(dto.Password)
              };

              _context.Users.Add(user);

              await _context.SaveChangesAsync();

              var responseDto = new UserResponseDto
              {
                  Id = user.Id,
                  Username = user.Username,
                  Email = user.Email
              };

              return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseDto);
          }*/
    }
}
