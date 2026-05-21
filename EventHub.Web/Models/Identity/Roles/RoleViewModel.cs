using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Identity.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol adı zorunludur.")]
        [Display(Name = "Rol Adı")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
