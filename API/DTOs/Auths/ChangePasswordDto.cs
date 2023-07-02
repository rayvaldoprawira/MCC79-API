using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Auths
{
    public class ChangePasswordDto
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
        [Required]
        public int Otp { get; set; }
        [PasswordPolicy]
        public string NewPassword { get; set; }

        [ConfirmPassword("NewPassword", ErrorMessage = "Password and Confirmation password didn't match.")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set;}
    }
}
