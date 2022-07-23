using AVS.API.Models;
using AVS.API.Services;
using AVS.API.Services.Contracts;
using AVS.API.Services.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AVS.API.Hubs
{
    // [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly static ConnectionMapping<string> connections = new ConnectionMapping<string>();

        private readonly TokenService tokenService;

        public ChatHub(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public string UserId { get; set; } = string.Empty;

        public override Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier?.ToString();

            // Console.WriteLine(context);

            if (UserId != null)
                connections.Add(UserId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // var context = Context.GetHttpContext();

            // if (context != null)
                // connections.Remove(tokenService.GetUserIdFromRequest(context), Context.ConnectionId);

            connections.Remove(UserId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        // 

        public async Task ReceivedMessage(ChatMessage message)
        {
            Console.WriteLine(message);

            await Clients.All.ReceivedMessage(message);
        }
    }
}