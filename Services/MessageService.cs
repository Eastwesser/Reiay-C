using Relay.Models;

public interface IMessageService
{
    Message GetMessage(int id);
    Message CreateMessage(MessageCreateDto messageDto);
}

public class MessageService : IMessageService
{
    public Message GetMessage(int id)
    {
        // Логика получения сообщения
        return new Message { Id = id, Content = "Пример сообщения", Timestamp = DateTime.Now };
    }

    public Message CreateMessage(MessageCreateDto messageDto)
    {
        // Логика создания сообщения
        return new Message { Content = messageDto.Content, Timestamp = DateTime.Now, UserId = messageDto.UserId };
    }
}
