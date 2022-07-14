using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace avs.api.Repositories
{
    public class ChatRepository
    {
        private readonly IMongoCollection<Chat> chats;        

        public ChatRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            chats = client
                .GetDatabase(options.Value.DatabaseName)
                .GetCollection<Chat>(options.Value.UsersCollectionName);
        }
    }
}