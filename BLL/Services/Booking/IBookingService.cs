using DAL.Entities;

namespace BLL
{
    // Rezervasyon işlemlerini tanımlayan servis arayüzü
    public interface IBookingService
    {
        // Belirtilen etkinliğe kullanıcı adına rezervasyon oluşturur
        public Task Book(int eventId, string userId, string displayName, string email, int quantity);
        // Verilen rezervasyon kimliğine göre rezervasyonu iptal eder
        public Task CancelBooking(int bookingId);
        // Belirtilen kimliğe sahip rezervasyonu getirir
        public Task<Booking> GetBookingById(int bookingId);
        // Belirtilen kullanıcıya ait tüm rezervasyonları listeler
        public Task<IEnumerable<Booking>> GetBookingsByUser(string userId);
    }
}
