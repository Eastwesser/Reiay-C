using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.DTOs;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _roleService.GetRoleAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleDto)
    {
        var role = await _roleService.CreateRoleAsync(roleDto);
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }
}
