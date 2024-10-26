using System.Threading.Tasks;
using Relay.Models;
using Relay.DTOs;
using Relay.Data;

namespace Relay.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                throw new InvalidOperationException("Role not found.");
            }
            return role;
        }

        public async Task<Role> CreateRoleAsync(RoleCreateDto roleDto)
        {
            var role = new Role { Name = roleDto.Name };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
