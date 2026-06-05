using DAL.Entities;
using System.ComponentModel.DataAnnotations;

// Bu ViewModel, yeni etkinlik oluşturma formundaki verileri taşır ve IValidatableObject ile özel doğrulama uygular.
namespace EventHub.Web.Models
{
    public class CreateEventViewModel : IValidatableObject
    {
        // Etkinliğin başlığını tutar; en fazla 100 karakter olabilir.
        [Required(ErrorMessage = "Etkinlik adı zorunludur.")]
        [Display(Name = "Etkinlik Adı")]
        [StringLength(100, ErrorMessage = "Etkinlik adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        // Etkinliği açıklayan metni tutar; kullanıcıya etkinlik hakkında ayrıntı sunar.
        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Açıklama")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;

        // Etkinliğin başlayacağı tarih ve saati tutar; bitiş tarihiyle birlikte doğrulanır.
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        // Etkinliğin sona ereceği tarih ve saati tutar; başlangıçtan sonra olması zorunludur.
        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        // Etkinliğin düzenleneceği yeri tutar; adres veya mekan adı olabilir.
        [Required(ErrorMessage = "Konum zorunludur.")]
        [Display(Name = "Konum")]
        [StringLength(200, ErrorMessage = "Konum en fazla 200 karakter olabilir.")]
        public string Location { get; set; } = string.Empty;

        // Bilet fiyatını tutar; 0 ücretsiz etkinliği ifade eder.
        [Display(Name = "Fiyat")]
        [Range(0, 1000000, ErrorMessage = "Fiyat 0 ile 1.000.000 arasında olmalıdır.")]
        public decimal Price { get; set; }

        // Etkinliğe katılabilecek maksimum kişi sayısını tutar.
        [Display(Name = "Kontenjan")]
        [Range(1, 1000, ErrorMessage = "Kontenjan 1 ile 1000 arasında olmalıdır.")]
        public int Capacity { get; set; }

        // Etkinliğin hangi kategoriye ait olduğunu tutar; DAL katmanındaki Category enum'undan gelir.
        [Display(Name = "Kategori")]
        public Category Category { get; set; }

        // Etkinlik için yüklenecek görsel dosyasını tutar; zorunlu değildir (null olabilir).
        [Display(Name = "Görsel")]
        public IFormFile? Image { get; set; }

        // Bitiş tarihinin başlangıç tarihinden sonra olmasını zorunlu kılan özel doğrulama metodu.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.",
                    new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}
