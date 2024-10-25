using Relay.DTOs;
using Relay.Models;
using System.Threading.Tasks;

namespace Relay.Services
{
    public interface IRoleService
    {
        Task<Role> GetRoleAsync(int id);
        Task<Role> CreateRoleAsync(RoleCreateDto roleDto);
    }
}
