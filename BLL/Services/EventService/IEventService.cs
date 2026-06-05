using BLL.DTOs;
using DAL.Entities;

namespace BLL
{
    // Etkinlik yönetimi işlemlerini tanımlayan servis arayüzü
    public interface IEventService
    {
        // Sistemdeki tüm etkinlikleri listeler
        Task<IEnumerable<Event>> GetAllEventsAsync();
        // Belirtilen kimliğe sahip etkinliği getirir
        Task<Event> GetEventByIdAsync(int id);

        // Mevcut etkinliklerde kullanılan benzersiz kategorileri döndürür
        Task<IEnumerable<Category>> GetCategoriesAsync();
        // DTO'dan gelen verilerle yeni bir etkinlik oluşturup kaydeder
        Task CreateEventAsync(CreatedEventDTO @event);

        // Var olan etkinlik kaydını günceller
        void UpdateEvent(Event @event);
        // Belirtilen kimliğe sahip etkinliği siler
        void DeleteEvent(int id);
    }
}
