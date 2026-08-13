using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.SharedDtos;
using TaskManagerAPI.DTOs.UserDTO;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly ICurrentUserService _currentUser;
        private const int DefaultOwnerId = 1;

        public UserService(AppDbContext context, PasswordService passwordService, ICurrentUserService currentUser)
        {
            _context = context;
            _passwordService = passwordService;
            _currentUser = currentUser;
        }

        //List of all users --------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }
            ).ToListAsync();
            return users;
        }

        // Get a specific user by Id -----------------------------------------------------------------------------------------
        public async Task<UserResponseDto> GetUser(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Id should be more than 0.");
            }

            var user = await _context.Users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("There is no user with the specified Id.");
            }

            return user;
        }

        // Get a specific user by Id with project details -------------------------------------------------------------------
        public async Task<UserWithProjectsDto> GetUserByIdWithDetails(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Id should be more than 0.");
            }

            var user = await _context.Users.Select(u => new UserWithProjectsDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                ProjectCount = u.Projects.Count(),
                Projects = u.Projects.Select(p => new ProjectSummaryDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    DeadLine = p.DeadLine,
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);


            if (user == null)
            {
                throw new Exception("There is no user with the specified Id.");
            }

            return user;
        }

        // Get a specific user by Id with project details and its tasks -----------------------------------------------------
        public async Task<UserWithProjectsAndTasksDto> GetUserByIdWithProjectsAndTasks(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Id should be more than 0.");
            }

            var user = await _context.Users.Select(u => new UserWithProjectsAndTasksDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Projects = u.Projects.Select(p => new FullProjectSummaryDto
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
                        IsCompleted = t.IsCompleted,
                    }).ToList(),
                }).ToList(),
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("There is no user with the specified Id.");
            }

            return user;

        }

        // Update an existing user -------------------------------------------------------------------------------------------
        public async Task UpdateUser(int id, UserUpdateDto updatedUserDto)
        {
            if (id <= DefaultOwnerId)
            {
                throw new Exception("Id should be more than 1.");
            }

            var query = _context.Users.AsQueryable();

            if (_currentUser.Role != "Admin")
            {
                query = query.Where(u => u.Id == _currentUser.UserId);
            }

            var user = await query.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new Exception("There is no user with the specified Id.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == updatedUserDto.Username && u.Id != id))
                throw new Exception("Username already exists.");

            if (await _context.Users.AnyAsync(u => u.Email == updatedUserDto.Email && u.Id != id))
                throw new Exception("Email already exists.");

            if (!string.IsNullOrWhiteSpace(updatedUserDto.Password))
            {
                user.PasswordHash = _passwordService.HashPassword(updatedUserDto.Password);
            }

            if (!string.IsNullOrWhiteSpace(updatedUserDto.Email))
            {
                user.Email = updatedUserDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(updatedUserDto.Username))
            {
                user.Username = updatedUserDto.Username;
            }

            await _context.SaveChangesAsync();

        }

        // Delete a user -----------------------------------------------------------------------------------------------------
        public async Task DeleteUser(int id)
        {
            if (id <= DefaultOwnerId)
            {
                throw new Exception("Id should be more than 1.");
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("There is no user with the specified Id.");
            }

            var userProject = await _context.Projects.Where(p => p.UserId == id).ToListAsync();

            foreach (var project in userProject)
            {
                project.UserId = DefaultOwnerId;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        //-------------------------------------------------------------------------------------------------------------------
    }
}
