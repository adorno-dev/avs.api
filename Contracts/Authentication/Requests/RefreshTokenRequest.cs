using System.ComponentModel.DataAnnotations;

namespace AVS.API.Contracts.Authentication.Requests
{
    public class RefreshTokenRequest
    {
        #region +Constructors

        public RefreshTokenRequest(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        #endregion

        #region +Properties

        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }

        #endregion
    }
}