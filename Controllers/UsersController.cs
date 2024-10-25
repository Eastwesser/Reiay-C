using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register(UserCreateDto userDto)
    {
        var user = _userService.Register(userDto);
        return Ok(user);
    }
}
