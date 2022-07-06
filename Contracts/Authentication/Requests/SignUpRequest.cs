using System.ComponentModel.DataAnnotations;

namespace AVS.API.Contracts.Authentication.Requests
{
    public class SignUpRequest
    {
        public SignUpRequest(string username, string email, string password, string confirmPassword)
        {
            Username = username;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }        
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}