using BLL;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Controllers
{
    // Rezervasyon işlemlerini yöneten controller — bilet satın alma, listeleme ve iptal işlemlerini kapsar
    // Yalnızca "Admin" veya "User" rolüne sahip giriş yapmış kullanıcılar bu controller'a erişebilir
    [Authorize(Roles = "Admin,User")]
    public class BookingController : Controller
    {
        // Etkinlik verilerine erişim sağlayan servis
        private readonly IEventService _eventService;
        // Rezervasyon CRUD işlemlerini gerçekleştiren servis
        private readonly IBookingService _bookingService;
        // Oturum açmış kullanıcının kimlik bilgilerini yöneten ASP.NET Identity servisi
        private readonly UserManager<IdentityUser> _userManager;

        // Bağımlılık enjeksiyonu ile gerekli servisler constructor aracılığıyla alınır
        public BookingController(
            IEventService eventService,
            IBookingService bookingService,
            UserManager<IdentityUser> userManager)
        {
            _eventService = eventService;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        // GET: /Booking/Index — Oturum açmış kullanıcının tüm rezervasyonlarını listeler
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userBookings = await _bookingService.GetBookingsByUser(currentUser?.Id ?? string.Empty);
            return View(userBookings);
        }



        // GET: /Booking/Create/{id} — Belirtilen etkinlik için rezervasyon oluşturma formunu gösterir
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var eventEntity = await _eventService.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var model = new CreateBookingViewModel
            {
                EventId = id,
                EventName = eventEntity.Name,
                EventPrice = eventEntity.Price
            };

            return View(model);
        }

        // POST: /Booking/Create — Formdaki verilerle rezervasyonu veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var currentUser = await _userManager.GetUserAsync(User);

            if (string.IsNullOrWhiteSpace(currentUser?.Id) ||
                string.IsNullOrWhiteSpace(currentUser.Email) ||
                string.IsNullOrWhiteSpace(currentUser.UserName))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı profilinizde geçerli bir e-posta adresi bulunmuyor.");
                return View(model);
            }

            try
            {
                await _bookingService.Book(
                    model.EventId,
                    currentUser.Id,
                    currentUser.UserName,
                    currentUser.Email,
                    model.Quantity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return View("CongratulationsView", model.EventName);
        }

        // GET: /Booking/Delete/{id} — Silinecek rezervasyonun onay sayfasını gösterir; yalnızca rezervasyon sahibi veya Admin erişebilir
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && !string.Equals(booking.UserId, currentUser?.Id, StringComparison.Ordinal))
            {
                return Forbid();
            }

            return View(booking);

        }

        // POST: /Booking/DeleteConfirmed — Rezervasyonu iptal eder; yalnızca rezervasyon sahibi veya Admin işlem yapabilir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingById(id);
                var currentUser = await _userManager.GetUserAsync(User);
                if (!User.IsInRole("Admin") && !string.Equals(booking.UserId, currentUser?.Id, StringComparison.Ordinal))
                {
                    return Forbid();
                }

                await _bookingService.CancelBooking(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            return RedirectToAction("Index", "Event");
        }
    }
}
