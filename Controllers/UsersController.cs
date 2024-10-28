using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Relay.Services;
using Relay.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto)
    {
        _logger.LogInformation("Регистрация нового пользователя с именем {Username}", userDto.Username);
        var user = await _userService.RegisterAsync(userDto);
        return Ok(user);
    }
}
