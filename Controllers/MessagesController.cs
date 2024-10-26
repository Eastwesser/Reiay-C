using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.DTOs;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessage(int id)
    {
        var message = await _messageService.GetMessageAsync(id);
        if (message == null) return NotFound();
        return Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto messageDto)
    {
        var message = await _messageService.CreateMessageAsync(messageDto);
        return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
    }
}
