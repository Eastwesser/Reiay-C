using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IRoleService roleService, ILogger<RolesController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    /// <summary>
    /// Получение роли по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        _logger.LogInformation("Получение роли ID {RoleId}", id);
        var role = await _roleService.GetRoleAsync(id);
        if (role == null)
        {
            _logger.LogWarning("Роль ID {RoleId} не найдена", id);
            return NotFound();
        }
        return Ok(role);
    }

    /// <summary>
    /// Создание новой роли.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleDto)
    {
        _logger.LogInformation("Создание новой роли с именем {RoleName}", roleDto.Name);
        var role = await _roleService.CreateRoleAsync(roleDto);
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }
}
