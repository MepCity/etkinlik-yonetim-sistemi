using BLL;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class BookingController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IBookingService _bookingService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(
            IEventService eventService,
            IBookingService bookingService,
            UserManager<IdentityUser> userManager)
        {
            _eventService = eventService;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userBookings = await _bookingService.GetBookingsByUser(currentUser?.Id ?? string.Empty);
            return View(userBookings);
        }
    
    
    
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
