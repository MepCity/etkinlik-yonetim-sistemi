using BLL.DTOs;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Services.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Etkinlik iş kurallarını uygulayan ve IEventService arayüzünü gerçekleştiren servis sınıfı
    public class EventService(IGenericRepository<Event> _eventRepository, IAttachmentService _attachmentService) : IEventService
    {
        // Veritabanındaki tüm etkinlik kayıtlarını getirir
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllAsync();
        }
        // Verilen kimliğe göre tek bir etkinlik kaydını getirir
        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }
        // DTO verilerinden yeni bir etkinlik nesnesi oluşturur; görsel varsa yükler ve veritabanına kaydeder
        public async Task CreateEventAsync(CreatedEventDTO @event)
        {
            var eventToAdd = new Event
            {
                Name = @event.Name,
                Description = @event.Description,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Capacity = @event.Capacity,
                Price = @event.Price,
                Location = @event.Location,
                Category = (Category)@event.Category, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (@event.Image != null)
                eventToAdd.ImageUrl = await _attachmentService.UploadAsync(@event.Image , "images");

            await _eventRepository.AddAsync(eventToAdd);
        }
        // Mevcut etkinlik nesnesini veritabanında günceller
        public  void UpdateEvent(Event @event)
        {
            _eventRepository.Update(@event);
        }
        // Belirtilen kimliğe sahip etkinliği veritabanından siler
        public void DeleteEvent(int id)
        {
            _eventRepository.Delete(id);
        }

        // Tüm etkinlikleri tarayarak kullanılan benzersiz kategorileri döndürür
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _eventRepository.GetAllAsync();
            return categories.Select(e => e.Category).Distinct();
        }
    }
}
