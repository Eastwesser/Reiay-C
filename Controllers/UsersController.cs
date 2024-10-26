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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto)
    {
        _logger.LogInformation("Registering new user.");
        var user = await _userService.RegisterAsync(userDto);
        return Ok(user);
    }
}
