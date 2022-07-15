using AVS.API.Models;
using AVS.API.Repositories;

namespace AVS.API.Services
{
    public class ChatService
    {
        private readonly ChatRepository repository;

        public ChatService(ChatRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Chat>> GetChatsAsync() => await repository.GetChatsAsync();

        public async Task<Chat> GetChatAsync(string id) => await repository.GetChatAsync(id);

        public string GetChatId(string[] users) => repository.GetChatByUsers(users);

        public string CreateChat(string ownerId, string[] users, ChatMessage? message = null)
        {
            var chat = new Chat(ownerId, users);

            repository.CreateChat(chat, message);

            return GetChatId(users);
        }

        public void CreateChatMessage(string chatId, ChatMessage message)
        {
            repository.CreateChatMessage(chatId, message);
        }
    }
}