using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    // Rezervasyon varlığı üzerinde veritabanı işlemlerini gerçekleştiren repository sınıfı
    public class BookingRepository : IGenericRepository<Booking>
    {
        // Veritabanı bağlamına erişim sağlayan alan
        private readonly ApplicationDbContext _applicationDbContext;

        // Bağımlılık enjeksiyonu ile veritabanı bağlamını alarak alanı başlatan yapıcı metot
        public BookingRepository(ApplicationDbContext applicationDbContext) => _applicationDbContext = applicationDbContext;

        // Tüm rezervasyonları ilgili etkinlik ve kullanıcı bilgileriyle birlikte asenkron olarak getirir
        public async Task<IEnumerable<Booking>> GetAllAsync() => await _applicationDbContext.Bookings
            .Include(booking => booking.Event)
            .Include(booking => booking.User)
            .ToListAsync();

        // Verilen ID'ye sahip rezervasyonu etkinlik ve kullanıcı bilgileriyle birlikte asenkron olarak getirir; bulunamazsa hata fırlatır
        public async Task<Booking> GetByIdAsync(int id)
        {
            var booking = await _applicationDbContext.Bookings
                .Include(currentBooking => currentBooking.Event)
                .Include(currentBooking => currentBooking.User)
                .FirstOrDefaultAsync(currentBooking => currentBooking.Id == id);
            return booking ?? throw new Exception($"Booking with ID {id} not found.");
        }

        // Yeni bir rezervasyonu veritabanına asenkron olarak ekler ve değişiklikleri kaydeder
        public async Task AddAsync(Booking booking)
        {
            await _applicationDbContext.Bookings.AddAsync(booking);
            await _applicationDbContext.SaveChangesAsync();
        }

        // Mevcut rezervasyonu bulur, bulunamazsa hata fırlatır; bulunursa günceller ve değişiklikleri kaydeder
        public void Update(Booking booking)
        {
            var existingBooking = _applicationDbContext.Bookings.FirstOrDefault(b => b.Id == booking.Id) ?? throw new Exception($"Booking with ID {booking.Id} not found.");
            _applicationDbContext.Update(booking);
            _applicationDbContext.SaveChanges();

        }

        // Verilen ID'ye sahip rezervasyonu bulur, bulunamazsa hata fırlatır; bulunursa siler ve değişiklikleri kaydeder
        public void Delete(int id)
        {
            var booking = _applicationDbContext.Bookings.FirstOrDefault(b => b.Id == id) ?? throw new Exception($"Booking with ID {id} not found.");
            _applicationDbContext.Bookings.Remove(booking);
            _applicationDbContext.SaveChanges();


        }


    }

}
