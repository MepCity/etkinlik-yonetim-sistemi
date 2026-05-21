using DAL.Entities;
using DAL.Repositories.Interfaces;
using Services.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly BookingMailService _bookingMailService;

        public BookingService(IGenericRepository<Booking> BookingRepository, IGenericRepository<Event> eventRepository, BookingMailService bookingMailService)
        {
            _eventRepository = eventRepository;
            _bookingRepository = BookingRepository;
            _bookingMailService = bookingMailService;
        }
        public async Task Book(int eventId, string userId, string displayName, string email, int quantity)
        {
            // 1. Checking availability of tickets
            var eventToBook = await _eventRepository.GetByIdAsync(eventId);
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("Etkinliğe katılmak için geçerli bir kullanıcı hesabı gereklidir.");

            if (quantity <= 0)
                throw new Exception("Katılım adedi sıfırdan büyük olmalıdır.");

            if (eventToBook.TicketsAvailable <= 0)
                throw new Exception("Bu etkinlik için kontenjan kalmamıştır.");

            if (quantity > eventToBook.TicketsAvailable)
                throw new Exception($"Bu etkinlik için yalnızca {eventToBook.TicketsAvailable} kişilik kontenjan kaldı.");

            var existingBooking = (await _bookingRepository.GetAllAsync())
                .Any(booking => booking.EventId == eventId && booking.UserId == userId);

            if (existingBooking)
                throw new Exception("Bu etkinliğe zaten katıldınız.");

            // 2. Creating a new booking
            var booking = new Booking
            {
                EventId = eventId,
                UserId = userId,
                CustomerName = displayName,
                Quantity = quantity,
                BookingDate = DateTime.Now,
                IsPaid = true
            };

            //3. Adding the booking to the database
            await _bookingRepository.AddAsync(booking);

            // 4. Send a confirmation mail to the user (best-effort — skipped if mail not configured)
            try
            {
                await _bookingMailService.SendBookingConfirmationAsync(
                    email,
                    eventToBook.Name,
                    eventToBook.StartDate.ToString("f"));
            }
            catch
            {
                // Email delivery failure does not cancel the booking
            }


        }

        public async Task<Booking> GetBookingById(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            return booking ?? throw new Exception("Katılım kaydı bulunamadı.");
        }

        public async Task CancelBooking(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId) ?? throw new Exception("Katılım kaydı bulunamadı.");
            _bookingRepository.Delete(bookingId);
        }


        public async Task<IEnumerable<Booking>> GetBookingsByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Enumerable.Empty<Booking>();

            var userBookings = await _bookingRepository.GetAllAsync();
            userBookings = userBookings.Where(booking => booking.UserId == userId).ToList();

            return userBookings;
        }
    }
}
