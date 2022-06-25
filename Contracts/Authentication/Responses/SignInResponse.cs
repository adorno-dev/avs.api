namespace AVS.API.Contracts.Authentication.Responses
{
    public class SignInResponse
    {
        #region +Constructors

        public SignInResponse(string? message)
        {
            Message = message;
        }

        public SignInResponse(string? token, string? refreshToken)
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

        public static SignInResponse NotAuthorized() => new SignInResponse("Invalid email or password.");

        public static SignInResponse EmailAlreadyExists() => new SignInResponse("Email already exists.");

        public static SignInResponse Ok(string token, string refreshToken) => new SignInResponse(token, refreshToken);  

        #endregion
    }
}