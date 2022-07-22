namespace AVS.API.Contracts.Authentication.Responses
{
    public class SignUpResponse
    {
        #region +Properties

        public string? Message { get; set; }

        public SignUpResponse(string? message)
        {
            Message = message;
        }

        #endregion

        #region +Extension Methods

        public static SignUpResponse MismatchOnPasswords() => new SignUpResponse("Mismatch on passwords.");

        #endregion
    }
}