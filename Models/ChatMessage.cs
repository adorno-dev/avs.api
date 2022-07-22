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

        [BsonId]
        [BsonElement("id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("timestamp")]
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [BsonElement("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }

        public override string ToString()
        {
            return $"[{TimeStamp}]:({Id}): {Body}";
        }
    }
}