using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AVS.API.Models
{
    public class Chat
    {
        public Chat(string ownerId, IList<string> users)
        {
            Users = users == null || users.Count < 2 ? 
                new List<string>() : users; 
            
            Messages = new List<ChatMessage>();
        }

        public Chat() 
        {
            Users = new List<string>();
            Messages = new List<ChatMessage>();
        }

        [BsonId]
        [BsonElement("id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("users")]
        [JsonPropertyName("users")]
        public IEnumerable<string> Users { get; set; }

        [BsonElement("messages")]
        [JsonPropertyName("messages")]
        public IEnumerable<ChatMessage> Messages { get; set; }
    }
}