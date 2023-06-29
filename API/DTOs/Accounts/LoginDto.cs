using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPolicyAttribute]
        public string Password { get; set; }
    }
}
