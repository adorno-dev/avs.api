using MongoDB.Bson.Serialization.Attributes;

namespace AVS.API.Models
{
    public class UserRefreshToken
    {
        public UserRefreshToken(string userId, string refreshToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
        }

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;
        public string RefreshToken { get; set; }    
    }
}