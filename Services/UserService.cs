using Microsoft.Extensions.Logging;
using Relay.Models;
using Relay.DTOs;
using System.Threading.Tasks;

namespace Relay.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserCreateDto userDto);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> RegisterAsync(UserCreateDto userDto)
        {
            _logger.LogInformation("Registering user in UserService.");
            var user = new User { Username = userDto.Username, Password = userDto.Password };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
