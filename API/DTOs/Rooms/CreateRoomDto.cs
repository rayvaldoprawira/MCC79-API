namespace API.DTOs.Rooms
{
    public class CreateRoomDto
    {
        public string Name { get; set; }
        public int Floor { get; set; }
        public int Capacity { get; set; }
    }
}
