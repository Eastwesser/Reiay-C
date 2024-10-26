using Relay.DTOs;
using Relay.Models;
using System.Threading.Tasks;

namespace Relay.Services
{
    public interface IMessageService
    {
        Task<Message> GetMessageAsync(int id);
        Task<Message> CreateMessageAsync(MessageCreateDto messageDto);
    }
}
