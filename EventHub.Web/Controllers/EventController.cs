using BLL;
using BLL.DTOs;
using DAL.Entities;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Attachments;
using System.Security.Claims;


namespace EventHub.Web.Controllers
{
    // Etkinlik listeleme, oluşturma, düzenleme, silme ve katılımcı görüntüleme işlemlerini yöneten controller
    // Primary constructor ile bağımlılıklar doğrudan tanımlanır: etkinlik servisi, rezervasyon servisi ve dosya yükleme servisi
    public class EventController(IEventService _eventService, IBookingService _bookingService, IAttachmentService _attachmentService) : Controller
    {
        // GET: /Event/Index — Tüm etkinlikleri listeler; kullanıcının hangi etkinliğe kayıt yaptırdığını işaretler
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllEventsAsync();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userBookings = await _bookingService.GetBookingsByUser(currentUserId ?? string.Empty);

            var eventViewModels = events.Select(e =>
            {
                var userBooking = userBookings.FirstOrDefault(b => b.EventId == e.Id);

                return new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Location = e.Location,
                    IsBookedByUser = userBooking != null,
                    BookingId = (userBooking?.Id) ?? 0,
                    Price = e.Price,
                    StartDate = e.StartDate,
                    TicketsAvailable = e.TicketsAvailable,
                    ImageUrl = e.ImageUrl,
                };
            }).ToList();

            ViewData["isAdmin"] = User.IsInRole("Admin");
            return View(eventViewModels);
        }

        // GET: /Event/Details/{id} — Belirtilen etkinliğin ayrıntılı bilgilerini gösterir
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _eventService.GetEventByIdAsync(id);
            if (eventDetails == null)
                return NotFound();

            return View(eventDetails);
        }

        // GET: /Event/Create — Yeni etkinlik oluşturma formunu gösterir; yalnızca Admin rolü erişebilir
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Event/Create — Formdaki verilerle yeni etkinliği veritabanına kaydeder; yalnızca Admin rolü işlem yapabilir
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel eventModel)
        {
            if (ModelState.IsValid)
            {
                var eventDto = new CreatedEventDTO
                {
                    Name = eventModel.Name,
                    Description = eventModel.Description,
                    StartDate = eventModel.StartDate,
                    EndDate = eventModel.EndDate,
                    Location = eventModel.Location,
                    Price = eventModel.Price,
                    Capacity = eventModel.Capacity,
                    Category = eventModel.Category,
                    Image = eventModel.Image,

                };

                await _eventService.CreateEventAsync(eventDto);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Etkinlik oluşturulamadı.");
            return View(eventModel);
        }

        // GET: /Event/Edit/{id} — Mevcut etkinliğin düzenleme formunu gösterir; yalnızca Admin rolü erişebilir
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDetails = await _eventService.GetEventByIdAsync(id);
            if (eventDetails == null)
                return NotFound();
            var eventModel = new CreateEventViewModel
            {
                Name = eventDetails.Name,
                Description = eventDetails.Description,
                StartDate = eventDetails.StartDate,
                EndDate = eventDetails.EndDate,
                Location = eventDetails.Location,
                Price = eventDetails.Price,
                Capacity = eventDetails.Capacity,
                Category = eventDetails.Category,


            };
            return View(eventModel);
        }

        // POST: /Event/Edit/{id} — Etkinlik bilgilerini günceller; yeni görsel yüklendiyse dosya sistemi de güncellenir; yalnızca Admin rolü işlem yapabilir
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEventViewModel eventModel)
        {
            if (ModelState.IsValid)
            {
                var eventDetails = await _eventService.GetEventByIdAsync(id);
                if (eventDetails == null)
                    return NotFound();

                eventDetails.Name = eventModel.Name;
                eventDetails.Description = eventModel.Description;
                eventDetails.StartDate = eventModel.StartDate;
                eventDetails.EndDate = eventModel.EndDate;
                eventDetails.Location = eventModel.Location;
                eventDetails.Price = eventModel.Price;
                eventDetails.Capacity = eventModel.Capacity;
                eventDetails.Category = eventModel.Category;
                eventDetails.UpdatedAt = DateTime.UtcNow;

                // Update image only if a new one is provided
                if (eventModel.Image != null)
                    eventDetails.ImageUrl = await _attachmentService.UploadAsync(eventModel.Image, "images");

                _eventService.UpdateEvent(eventDetails);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Etkinlik güncellenemedi.");
            return View(eventModel);
        }


        // GET: /Event/Delete/{id} — Silinecek etkinliğin onay sayfasını gösterir; yalnızca Admin rolü erişebilir
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _eventService.GetEventByIdAsync(id);
            if (eventDetails == null)
                return NotFound();
            return View(eventDetails);
        }

        // POST: /Event/DeleteConfirmed — Etkinliği ve varsa ilişkili görsel dosyasını kalıcı olarak siler; yalnızca Admin rolü işlem yapabilir
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventDetails = await _eventService.GetEventByIdAsync(id);
            if (eventDetails == null)
                return NotFound();

            // Checking if the event have image
            if (eventDetails.ImageUrl != null)
            {
                // Delete the image from the server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", eventDetails.ImageUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }


            _eventService.DeleteEvent(id);
            return RedirectToAction("Index");

        }

        // GET: /Event/Attendees/{id} — Belirli bir etkinliğe kayıt yaptıran kullanıcıların listesini gösterir; yalnızca Admin rolü erişebilir
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Attendees(int id)
        {
            var eventDetails = await _eventService.GetEventByIdAsync(id);
            if (eventDetails == null)
                return NotFound();

            return View(eventDetails);
        }
    }
}
