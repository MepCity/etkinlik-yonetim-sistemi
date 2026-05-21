using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventHub.Web.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TicketsAvailable { get; set; }
        public int BookingId { get; set; }
        public bool IsBookedByUser { get; set; }
        public string? ImageUrl { get; set; }
    }

}
