using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Models
{
    public class LoginUser
    {
        public LoginUser()
        {
        }

        public LoginUser(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password is too short.")]
        public string Password { get; set; }

    }
}