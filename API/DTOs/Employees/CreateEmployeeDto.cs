using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees
{
    public class CreateEmployeeDto
    {
/*        [Required]
        public string Nik { get; set; }*/
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Range (0, 1)]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
