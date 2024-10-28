using System.Threading.Tasks;
using Relay.Models;
using Relay.DTOs;

namespace Relay.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserCreateDto userDto); // Регистрация нового пользователя
    }
}
