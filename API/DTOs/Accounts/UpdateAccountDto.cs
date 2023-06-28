using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        [Required]
        public Guid Guid { get; set; }
        [PasswordPolicyAttribute]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int Otp { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
