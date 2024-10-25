using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.Models;

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
        var role = _roleService.GetRole(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public IActionResult CreateRole(RoleCreateDto roleDto)
    {
        var role = _roleService.CreateRole(roleDto);
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }
}
