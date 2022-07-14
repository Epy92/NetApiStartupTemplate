using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class AuthenticateViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
