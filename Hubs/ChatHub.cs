using AVS.API.Models;
using AVS.API.Services;
using AVS.API.Services.Contracts;
using AVS.API.Services.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AVS.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly static ConnectionMapping<string> connections = new ConnectionMapping<string>();

        private readonly TokenService tokenService;
        
        public string UserId { get; set; } = string.Empty;

        public ChatHub(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public override Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier?.ToString();

            if (UserId != null)
                connections.Add(UserId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId = Context.UserIdentifier?.ToString();

            if (UserId != null)
                connections.Remove(UserId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task ReceivedMessage(ChatMessage message)
        {
            await Clients.Client(message.SenderId).ReceivedMessage(message);
        }
    }
}