using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Bookings
{
    public class UpdateBookingDto
    {
        [Required]
        public Guid Guid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, 4, ErrorMessage = "Request = 0, Reject = 1, UpComing = 2, OnGoing = 3, Done = 4 ")]
        public StatusLevel Status { get; set; }
        public string Remarks { get; set; }
        [Required]
        public Guid RoomGuid { get; set; }
        [Required]
        public Guid EmployeeGuid { get; set; }
    }
}
