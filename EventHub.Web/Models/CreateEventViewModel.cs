using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
    public class CreateEventViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Etkinlik adı zorunludur.")]
        [Display(Name = "Etkinlik Adı")]
        [StringLength(100, ErrorMessage = "Etkinlik adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [Display(Name = "Açıklama")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        [Display(Name = "Konum")]
        [StringLength(200, ErrorMessage = "Konum en fazla 200 karakter olabilir.")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Fiyat")]
        [Range(0, 1000000, ErrorMessage = "Fiyat 0 ile 1.000.000 arasında olmalıdır.")]
        public decimal Price { get; set; }

        [Display(Name = "Kontenjan")]
        [Range(1, 1000, ErrorMessage = "Kontenjan 1 ile 1000 arasında olmalıdır.")]
        public int Capacity { get; set; }

        [Display(Name = "Kategori")]
        public Category Category { get; set; }

        [Display(Name = "Görsel")]
        public IFormFile? Image { get; set; }

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
