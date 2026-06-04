using System.ComponentModel.DataAnnotations;
using EventHub.Web.Validation;

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
        public decimal EventPrice { get; set; }

        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [Display(Name = "Kart Numarası")]
        [Luhn]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart sahibi adı zorunludur.")]
        [Display(Name = "Kart Sahibi Adı")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad en az 3 karakter olmalıdır.")]
        public string CardHolderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
        [Display(Name = "Son Kullanma Tarihi (AA/YY)")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Geçerli format: AA/YY")]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required(ErrorMessage = "CVV zorunludur.")]
        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV 3 veya 4 haneli olmalıdır.")]
        public string Cvv { get; set; } = string.Empty;
    }
}
