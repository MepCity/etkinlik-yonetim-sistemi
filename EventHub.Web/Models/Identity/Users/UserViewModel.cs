using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Identity.Users
{
    public class UserViewModel
    {
        [Display(Name = "Kimlik")]
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Görünen Ad")]
        public string DisplayName { get; set; } = string.Empty;

        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Roller")]
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
