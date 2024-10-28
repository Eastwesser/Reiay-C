using System.Threading.Tasks;
using Relay.Models;
using Relay.DTOs;
using Relay.Data;
using Microsoft.Extensions.Logging;

namespace Relay.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        public async Task<User> RegisterAsync(UserCreateDto userDto)
        {
            _logger.LogInformation("Регистрация нового пользователя именем {Username}", userDto.Username);
            var user = new User { Username = userDto.Username!, Password = userDto.Password! };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Пользователь зарегистрирован ID {UserId}", user.Id);
            return user;
        }
    }
}
