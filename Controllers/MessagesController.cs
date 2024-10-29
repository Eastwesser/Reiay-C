using Microsoft.AspNetCore.Mvc;
using Relay.Services;
using Relay.DTOs;
using Relay.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading.Tasks;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessage(int id)
    {
        var culture = HttpContext.Items["RequestCulture"] as CultureInfo ?? new CultureInfo("ru");
        _logger.LogInformation("Получение сообщения на языке: {Culture}", culture.Name);

        var message = await _messageService.GetMessageAsync(id);
        return message == null ? NotFound() : Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto messageDto)
    {
        var createdMessage = await _messageService.CreateMessageAsync(messageDto);

        return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.Id }, createdMessage);
    }
}
