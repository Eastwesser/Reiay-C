using System.Threading.Tasks;
using Relay.Models;
using Relay.DTOs;

namespace Relay.Services
{
    public interface IRoleService
    {
        Task<Role> GetRoleAsync(int id);
        Task<Role> CreateRoleAsync(RoleCreateDto roleDto);
    }
}
