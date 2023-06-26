using API.Contracts;
using API.Data;
using API.Models;
using System.Runtime.InteropServices;

namespace API.Repositories;

public class RoomRepository : GeneralRepository<Room>, IRoomRepository
{
    public RoomRepository(BookingDbContext context) : base(context)
    {
    }


}
