using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : GeneralController<Booking>
    {
        private readonly IBookingRepository _repository;

        public BookingController(IBookingRepository repository) : base(repository)
        {
            _repository = repository;
        }

    }
}
