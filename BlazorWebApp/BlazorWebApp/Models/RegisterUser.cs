using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Models
{
    public class RegisterUser
    {
        public RegisterUser()
        {
        }

        public RegisterUser(string firstname, string lastname, string email, string password)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Email = email;
            this.Password = password;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password is too short.")]
        public string Password { get; set; }
    }
}