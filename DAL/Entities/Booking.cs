using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!;

        [Display(Name = "Katılımcı")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Adet")]
        public int Quantity { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime BookingDate { get; set; }
        public bool IsPaid { get; set; }
    }

}
