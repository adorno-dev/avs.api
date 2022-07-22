using AVS.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace AVS.API.Hubs
{
    public class ChatHub : Hub<IChatClient> //Hub
    {
        public async Task ReceivedMessage(ChatMessage message)
        {
            Console.WriteLine(message);

            await Clients.All.ReceivedMessage(message);
        }
    }

    public interface IChatClient
    {
        Task ReceivedMessage(ChatMessage message);    
    }
}