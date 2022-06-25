using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AVS.API.Repositories
{
    public class UserRepository
    {
        #region +Private Methods

        private readonly IMongoCollection<User> users;        

        #endregion

        #region +Constructors
        
        public UserRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            users = client
                .GetDatabase(options.Value.DatabaseName)
                .GetCollection<User>(options.Value.UsersCollectionName);
        }

        #endregion

        #region +Public Methods

        public List<User>? GetAll() => users
            .Find(_ => true)
            .Project(s => new User(s.Username, s.Email, null) { Id = s.Id, Creation = null })
            .ToList();

        public User? Get(string email) => users.Find(x => x.Email == email).FirstOrDefault();

        public void Create(User user) => users.InsertOne(user);

        #endregion

        #region +Extension Methods

        public void Get(string email, out User? user) => user = Get(email);

        #endregion
    }
}