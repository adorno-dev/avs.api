using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AVS.API.Repositories
{
    public class ChatRepository
    {
        private readonly IMongoCollection<Chat> chats;        

        public ChatRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            chats = client
                .GetDatabase(options.Value.DatabaseName)
                .GetCollection<Chat>(options.Value.ChatsCollectionName);
        }

        public async Task<List<Chat>> GetChatsAsync() => await chats.Find(_ => true).ToListAsync();

        public async Task<Chat> GetChatAsync(string id) => await chats.Find(c => c.Id.Equals(id)).FirstOrDefaultAsync();

        public string GetChatByUsers(string[] users)
        {
            var findByUsers = Builders<Chat>.Filter.Eq(c => c.Users, users);
            var chatId = chats.Find(findByUsers).Project(s => s.Id).FirstOrDefault();
            return chatId;
        }

        public void CreateChat(Chat chat, ChatMessage? message = null)
        {
            if (message != null)
                chat.Messages = new List<ChatMessage> { message };

            chats.InsertOne(chat);
        }

        public void CreateChatMessage(string chatId, ChatMessage message)
        {
            var filter = Builders<Chat>.Filter.Eq(x => x.Id, chatId);
            var update = Builders<Chat>.Update.AddToSet(x => x.Messages, message);

            chats.UpdateOne(filter, update);
        }
    }
}