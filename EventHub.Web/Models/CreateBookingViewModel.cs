using System.ComponentModel.DataAnnotations;
using EventHub.Web.Validation;

// Bu ViewModel, kullanıcının bilet satın alma (rezervasyon oluşturma) formundaki verileri taşır.
namespace EventHub.Web.Models
{
    public class CreateBookingViewModel
    {
        // Kullanıcının kaç bilet almak istediğini tutar; en az 1 olması zorunludur.
        [Required(ErrorMessage = "Katılım adedi zorunludur.")]
        [Display(Name = "Katılım Adedi")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir katılım adedi girin.")]
        public int Quantity { get; set; }

        // Rezervasyon yapılacak etkinliğin veritabanı kimliğini tutar; form aracılığıyla gizli alan olarak iletilir.
        public int EventId { get; set; }

        // Ödeme sayfasında görüntülemek için etkinlik adını tutar.
        public string EventName { get; set; } = string.Empty;

        // Toplam tutarı hesaplamak için etkinliğin birim fiyatını tutar.
        public decimal EventPrice { get; set; }

        // Kredi/banka kartı numarasını tutar; Luhn algoritmasıyla doğrulanır.
        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [Display(Name = "Kart Numarası")]
        [Luhn]
        public string CardNumber { get; set; } = string.Empty;

        // Kart üzerindeki isim bilgisini tutar; en az 3 karakter olması gerekir.
        [Required(ErrorMessage = "Kart sahibi adı zorunludur.")]
        [Display(Name = "Kart Sahibi Adı")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad en az 3 karakter olmalıdır.")]
        public string CardHolderName { get; set; } = string.Empty;

        // Kartın son kullanma tarihini AA/YY formatında tutar; regex ile format doğrulanır.
        [Required(ErrorMessage = "Son kullanma tarihi zorunludur.")]
        [Display(Name = "Son Kullanma Tarihi (AA/YY)")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Geçerli format: AA/YY")]
        public string ExpiryDate { get; set; } = string.Empty;

        // Kartın güvenlik kodunu (CVV/CVC) tutar; 3 veya 4 haneli olmalıdır.
        [Required(ErrorMessage = "CVV zorunludur.")]
        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV 3 veya 4 haneli olmalıdır.")]
        public string Cvv { get; set; } = string.Empty;
    }
}
