using API.Contracts;
using API.DTOs.Bookings;
using API.Models;

namespace API.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public BookingService(IBookingRepository bookingRepository,
                              IRoomRepository roomRepository,
                              IEmployeeRepository employeeRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<GetBookingDto>? GetBooking()
        {
            var bookings = _bookingRepository.GetAll();
            if (!bookings.Any())
            {
                return null; // No Booking  found
            }

            var toDto = bookings.Select(booking =>
                                                new GetBookingDto
                                                {
                                                    Guid = booking.Guid,
                                                    StartDate = booking.StartDate,
                                                    EndDate = booking.EndDate,
                                                    Status = booking.Status,
                                                    Remarks = booking.Remarks,
                                                    RoomGuid = booking.RoomGuid,
                                                    EmployeeGuid = booking.EmployeeGuid
                                                }).ToList();

            return toDto; // Booking found
        }

        public GetBookingDto? GetBooking(Guid guid)
        {
            var booking = _bookingRepository.GetByGuid(guid);
            if (booking is null)
            {
                return null; // booking not found
            }

            var toDto = new GetBookingDto
            {
                Guid = booking.Guid,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks,
                RoomGuid = booking.RoomGuid,
                EmployeeGuid = booking.EmployeeGuid
            };

            return toDto; // bookings found
        }

        public GetBookingDto? CreateBooking(CreateBookingDto newBookingDto)
        {
            var booking = new Booking
            {
                Guid = new Guid(),
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdBooking = _bookingRepository.Create(booking);
            if (createdBooking is null)
            {
                return null; // Booking not created
            }

            var toDto = new GetBookingDto
            {
                Guid = createdBooking.Guid,
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
            };

            return toDto; // Booking created
        }

        public int UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

            var booking = new Booking
            {
                Guid = updateBookingDto.Guid,
                StartDate = updateBookingDto.StartDate,
                EndDate = updateBookingDto.EndDate,
                Status = updateBookingDto.Status,
                Remarks = updateBookingDto.Remarks,
                RoomGuid = updateBookingDto.RoomGuid,
                EmployeeGuid = updateBookingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now,
                CreatedDate = getBooking!.CreatedDate
            };

            var isUpdate = _bookingRepository.Update(booking);
            if (!isUpdate)
            {
                return 0; // Booking not updated
            }

            return 1;
        }

        public int DeleteBooking(Guid guid)
        {
            var isExist = _bookingRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var booking = _bookingRepository.GetByGuid(guid);
            var isDelete = _bookingRepository.Delete(booking!);
            if (!isDelete)
            {
                return 0; // Booking not deleted
            }

            return 1;
        }

        public IEnumerable<BookingsLengthDto>? BookingDuration()
        {
            var bookings = _bookingRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var entities = (from booking in bookings
                            join room in rooms on booking.RoomGuid equals room.Guid
                            select new
                            {
                                guid = room.Guid,
                                startDate = booking.StartDate,
                                endDate = booking.EndDate,
                                roomName = room.Name
                            }).ToList();

            var bookingDurations = new List<BookingsLengthDto>();

            foreach (var entity in entities)
            {
                TimeSpan duration = entity.endDate - entity.startDate;

                // Count the number of weekends within the duration
                int totalDays = (int)duration.TotalDays;
                int weekends = 0;

                for (int i = 0; i <= totalDays; i++)
                {
                    var currentDate = entity.startDate.AddDays(i);
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekends++;
                    }
                }

                // Calculate the duration without weekends
                TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);

                var bookingDurationDto = new BookingsLengthDto
                {
                    RoomGuid = entity.guid,
                    RoomName = entity.roomName,
                    BookingLength = bookingLength
                };

                bookingDurations.Add(bookingDurationDto);
            }

            return bookingDurations;
        }

        public List<BookingsDetailDto>? GetBookingDetais()
        {
            var booking = _bookingRepository.GetBookingDetails();
            var bookingDetails = booking.Select(b => new BookingsDetailDto
            {
                Guid = b.Guid,
                BookedNik = b.BookedNik,
                BookedBy = b.BookedBy,
                RoomName = b.RoomName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status,
                Remarks = b.Remarks
            }).ToList();

            return bookingDetails;
        }

        public BookingsDetailDto? GetBookingDetailsByGuid(Guid guid)
        {
            var relatedBooking = GetBookingDetais().SingleOrDefault(b => b.Guid == guid);
            return relatedBooking;
        }

        public IEnumerable<BookingsTodayDto>? BookingToday()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings is null)
            {
                return null;
            }

            var employees = _employeeRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var bookingToday = (from booking in bookings
                                join employee in employees on booking.EmployeeGuid equals employee.Guid
                                join room in rooms on booking.RoomGuid equals room.Guid
                                where booking.StartDate <= DateTime.Now.Date && booking.EndDate >= DateTime.Now
                                select new BookingsTodayDto
                                {
                                    BookingGuid = booking.Guid,
                                    RoomName = room.Name,
                                    Status = booking.Status,
                                    Floor = room.Floor,
                                    BookedBy = employee.FirstName + " " + employee.LastName
                                }).ToList();

            if (!bookingToday.Any())
            {
                return null;
            }

            return bookingToday;
        }
    }
}
