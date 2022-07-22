using AVS.API.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace AVS.API.Clients
{
    public class ChatClient
    {
        private const string URL = "https://localhost:5000/message";

        private readonly HubConnection connection;

        public ChatClient()
        {
            connection = new HubConnectionBuilder().WithUrl(URL).Build();
        }

        public async Task Start()
        {
            await connection.StartAsync();
        }

        public async Task Dispose()
        {
            await connection.DisposeAsync();
        }

        public async Task Watch()
        {
            await foreach (var message in connection.StreamAsync<ChatMessage>("NewMessage"))
                Console.WriteLine(message.Body);
        }

        public async Task Send(ChatMessage message)
        {
            await connection.SendAsync("NewMessage", message);
        }
    }
}