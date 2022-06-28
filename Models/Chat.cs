using MongoDB.Bson.Serialization.Attributes;

namespace AVS.API.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public IEnumerable<string>? Users { get; set; }

        public IEnumerable<Message>? Messages { get; set; }
    }

    public class Message
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public DateTime? Timestamp { get; set; }

        public string? Author { get; set; }

        public string? Content { get; set; }
    }
}