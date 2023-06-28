using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class CreateUniversityDto
{
    [Required]
    public string Code { get; set; }
    public string Name { get; set; }
}
