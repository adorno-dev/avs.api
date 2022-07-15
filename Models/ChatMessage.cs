using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AVS.API.Models
{
    public class ChatMessage
    {
        public ChatMessage(string ownerId, string body)
        {
            this.TimeStamp = DateTime.UtcNow;
            this.SenderId = ownerId;
            this.Body = body;
        }

        [BsonElement("timestamp")]
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        [BsonId]
        [BsonElement("id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [BsonElement("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}