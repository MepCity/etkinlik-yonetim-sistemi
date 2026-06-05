using System.ComponentModel.DataAnnotations;

// Bu ViewModel, yeni bir kullanıcı rolü oluşturma formundan gelen veriyi taşır.
namespace EventHub.Web.Models.Identity.Roles
{
    public class RoleViewModel
    {
        // Oluşturulacak rolün adını tutar; zorunludur ve en fazla 100 karakter olabilir.
        [Required(ErrorMessage = "Rol adı zorunludur.")]
        [Display(Name = "Rol Adı")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
