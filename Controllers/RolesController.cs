using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.Models;
using Relay.DTOs;  // Добавьте это пространство имён для доступа к RoleCreateDto

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
    public IActionResult GetRole(int id)
    {
        var role = _roleService.GetRoleAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public IActionResult CreateRole(RoleCreateDto roleDto)
    {
        var role = _roleService.CreateRoleAsync(roleDto);
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }
}
