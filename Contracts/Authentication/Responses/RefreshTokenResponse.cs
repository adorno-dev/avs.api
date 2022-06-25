namespace AVS.API.Contracts.Authentication.Responses
{
    public class RefreshTokenResponse
    {
        #region +Constructors

        public RefreshTokenResponse(string? message)
        {
            Message = message;
        }

        public RefreshTokenResponse(string? token, string? refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        #endregion

        #region +Properties

        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        #endregion

        #region +Extension Methods

        public static RefreshTokenResponse InvalidRefreshToken() => new RefreshTokenResponse("Invalid refresh token.");

        public static RefreshTokenResponse TokenNotExpired() => new RefreshTokenResponse("This token hasn't expired yet.");

        public static RefreshTokenResponse Ok(string token, string refreshToken) => new RefreshTokenResponse(token, refreshToken);

        #endregion
    }
}