namespace API.DTOs.Bookings
{
    public class BookingsLengthDto
    {
        public Guid RoomGuid { get; set; }
        public string RoomName { get; set; }
        public TimeSpan BookingLength { get; set; }

    }
}
