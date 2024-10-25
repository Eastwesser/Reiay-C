using Relay.Models;
using Relay.DTOs;

namespace Relay.Services
{
    public interface IMessageService
    {
        Task<Message> GetMessageAsync(int id); // Измените метод на асинхронный
        Task<Message> CreateMessageAsync(MessageCreateDto messageDto); // Измените метод на асинхронный
    }

    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Message> CreateMessageAsync(MessageCreateDto messageDto)
        {
            var message = new Message { Content = messageDto.Content, UserId = messageDto.UserId, Timestamp = DateTime.Now };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
