using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
    public class CreateBookingViewModel
    {
        [Required(ErrorMessage = "Katılım adedi zorunludur.")]
        [Display(Name = "Katılım Adedi")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir katılım adedi girin.")]
        public int Quantity { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
    }
}
