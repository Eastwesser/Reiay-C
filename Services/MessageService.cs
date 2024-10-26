using System;
using System.Threading.Tasks;
using Relay.Data;
using Relay.DTOs;
using Relay.Models;

namespace Relay.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                throw new InvalidOperationException("Message not found.");
            }
            return message;
        }

        public async Task<Message> CreateMessageAsync(MessageCreateDto messageDto)
        {
            var message = new Message
            {
                Content = messageDto.Content,
                UserId = messageDto.UserId,
                Timestamp = DateTime.Now
            };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
