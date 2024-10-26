using System.Threading.Tasks;
using Relay.Models;
using Relay.DTOs;
using Relay.Data;

namespace Relay.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(UserCreateDto userDto)
        {
            var user = new User { Username = userDto.Username!, Password = userDto.Password! };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
