using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.Models;
using Relay.DTOs;  // Добавьте это пространство имён для доступа к MessageCreateDto

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
    public IActionResult GetMessage(int id)
    {
        var message = _messageService.GetMessage(id);
        if (message == null) return NotFound();
        return Ok(message);
    }

    [HttpPost]
    public IActionResult CreateMessage(MessageCreateDto messageDto)
    {
        var message = _messageService.CreateMessage(messageDto);
        return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
    }
}
