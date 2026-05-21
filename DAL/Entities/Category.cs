using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public enum Category : byte
    {
        [Display(Name = "Festival")]
        Festival = 1,
        [Display(Name = "Spor")]
        Sport,
        [Display(Name = "Eğitim")]
        Education,
        [Display(Name = "Ulusal")]
        National,
        [Display(Name = "Sanat")]
        Art,
        [Display(Name = "Konferans")]
        Conference,
        [Display(Name = "Yemek")]
        Food,
        [Display(Name = "Yardım")]
        Charity,
    }
}
