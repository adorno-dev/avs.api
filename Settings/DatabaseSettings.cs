namespace AVS.API.Settings
{
    public class DatabaseSettings
    {
        public string? ConnectionString { get; set; }     
        public string? DatabaseName { get; set; }
        public string? UsersCollectionName { get; set; }
        public string? UserRefreshTokensCollectionName { get; set; }
        public string? ChatsCollectionName { get; set; }
    }
}