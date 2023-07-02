using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Bookings
{
    public class CreateBookingDto
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range (0, 4)]
        public StatusLevel Status { get; set; }
        public string Remarks { get; set; }
        [Required]
        public Guid RoomGuid { get; set; }
        [Required]
        public Guid EmployeeGuid { get; set; }
    }
}
