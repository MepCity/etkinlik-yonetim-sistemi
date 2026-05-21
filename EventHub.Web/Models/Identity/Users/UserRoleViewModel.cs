using EventHub.Web.Models.Identity.Roles;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Identity.Users
{
    public class UserRoleViewModel
    {
        [Display(Name = "Kullanıcı Kimliği")]
        public string UserId { get; set; } = string.Empty;
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;
        public List<UpdateRoleViewModel> Roles { get; set; } = [];
    }
}
