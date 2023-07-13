using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees
{
    public class ChartDto
    {
        public Guid Guid { get; set; }

        public string FullName { get; set; }

        public string UniversityName { get; set; }
    }
}
