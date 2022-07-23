using AVS.API.Models;

namespace AVS.API.Services.Contracts
{
    public interface IChatClient
    {
        Task ReceivedMessage(ChatMessage message);    
    }
}