using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// Получение сообщения по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessage(int id)
    {
        _logger.LogInformation("Получение сообщения ID {MessageId}", id);
        var message = await _messageService.GetMessageAsync(id);
        if (message == null)
        {
            _logger.LogWarning("Сообщение ID {MessageId} не найдено", id);
            return NotFound();
        }
        return Ok(message);
    }

    /// <summary>
    /// Создание нового сообщения.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto messageDto)
    {
        _logger.LogInformation("Создание нового сообщения для пользователя с ID {UserId}", messageDto.UserId);
        var message = await _messageService.CreateMessageAsync(messageDto);
        return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
    }
}
