using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    // Etkinlik varlığı üzerinde veritabanı işlemlerini gerçekleştiren repository sınıfı
    public class EventRepository(ApplicationDbContext applicationDbContext) : IGenericRepository<Event>
    {
        // Veritabanı bağlamına erişim sağlayan alan
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        // Tüm etkinlikleri rezervasyonlarıyla birlikte asenkron olarak getirir
        public async Task<IEnumerable<Event>> GetAllAsync() => await _applicationDbContext.Events.Include(e => e.Bookings).ToListAsync();

        // Verilen ID'ye sahip etkinliği rezervasyonlarıyla birlikte asenkron olarak getirir; bulunamazsa hata fırlatır
        public async Task<Event> GetByIdAsync(int id)
        {
            var eventEntity = await _applicationDbContext.Events.Include(e => e.Bookings).FirstOrDefaultAsync(e => e.Id == id);
            return eventEntity ?? throw new Exception($"Event with ID {id} not found.");
        }

        // Yeni bir etkinliği veritabanına asenkron olarak ekler ve değişiklikleri kaydeder
        public async Task AddAsync(Event applicationEvent)
        {
              await _applicationDbContext.Events.AddAsync(applicationEvent);
              await _applicationDbContext.SaveChangesAsync();
        }

        // Mevcut bir etkinliği güncelleyerek değişiklikleri veritabanına kaydeder
        public void Update(Event applicationEvent)
        {
            _applicationDbContext.Update(applicationEvent);
            _applicationDbContext.SaveChanges();
        }

        // Verilen ID'ye sahip etkinliği bulur, bulunamazsa hata fırlatır; bulunursa siler ve değişiklikleri kaydeder
        public void Delete(int id)
        {
            var eventEntity = _applicationDbContext.Events.FirstOrDefault(e => e.Id == id) ?? throw new Exception($"Event with ID {id} not found.");
            _applicationDbContext.Events.Remove(eventEntity);
            _applicationDbContext.SaveChanges();
        }




    }
}
