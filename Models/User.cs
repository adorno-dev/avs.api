using MongoDB.Bson.Serialization.Attributes;

namespace AVS.API.Models
{
    public class User
    {
        public User(string username, string email, string? password = null)
        {
            Creation = DateTime.UtcNow;

            Username = username;
            Email = email;

            if (password is not null)
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public DateTime? Creation { get; set; }

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; }        
        public string Email { get; set; }
        public string? Password { get; set; }

        public void ClearPassword() => Password = null;

        public bool VerifyPassword(string password) => BCrypt.Net.BCrypt.EnhancedVerify(password, Password);
    }
}