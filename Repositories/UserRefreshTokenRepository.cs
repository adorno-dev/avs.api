using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AVS.API.Repositories
{
    public class UserRefreshTokenRepository
    {
        #region +Private Fields

        private readonly IMongoCollection<UserRefreshToken> userRefreshTokens;

        #endregion

        #region +Constructors

        public UserRefreshTokenRepository(IOptions<DatabaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            userRefreshTokens = client
                .GetDatabase(options.Value.DatabaseName)
                .GetCollection<UserRefreshToken>(options.Value.UserRefreshTokensCollectionName);
        }

        #endregion

        #region +Public Methods

        public string? Get(string userId)
        {
            var userRefreshToken = userRefreshTokens.Find(x => x.UserId == userId).FirstOrDefault();

            return (userRefreshToken is null) ? null : userRefreshToken.RefreshToken;
        }

        public void Save(string userId, string refreshToken)
        {
            var userRefreshToken = userRefreshTokens.Find(x => x.UserId == userId).FirstOrDefault();

            if (userRefreshToken is not null)
                userRefreshTokens.ReplaceOne(x => x.UserId == userId, new UserRefreshToken(userId, refreshToken));
            else
                userRefreshTokens.InsertOne(new UserRefreshToken(userId, refreshToken));
        }

        public void Delete(string userId, string refreshToken)
            => userRefreshTokens.DeleteOne(x => x.UserId == userId && x.RefreshToken == refreshToken);
        
        #endregion
    
        #region +Extension Methods

        public void Get(string userId, out string? refreshToken) => refreshToken = Get(userId);

        #endregion
    }
}