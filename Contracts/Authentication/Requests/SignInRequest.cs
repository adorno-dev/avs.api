using System.ComponentModel.DataAnnotations;

namespace AVS.API.Contracts.Authentication.Requests
{
    public class SignInRequest
    {
        public SignInRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }        
        [Required]
        public string Password { get; set; }
    }
}