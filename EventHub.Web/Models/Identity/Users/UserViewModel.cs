using System.ComponentModel.DataAnnotations;

// Bu ViewModel, kullanıcı yönetim listesinde her bir kullanıcıya ait özet bilgileri taşır.
namespace EventHub.Web.Models.Identity.Users
{
    public class UserViewModel
    {
        // Kullanıcının Identity sistemindeki benzersiz kimliğini tutar; düzenleme/silme işlemlerinde kullanılır.
        [Display(Name = "Kimlik")]
        public string Id { get; set; } = string.Empty;

        // Kullanıcının arayüzde gösterilen tam adını tutar (ad soyad gibi).
        [Display(Name = "Görünen Ad")]
        public string DisplayName { get; set; } = string.Empty;

        // Kullanıcının kayıt ve giriş için kullandığı e-posta adresini tutar.
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        // Kullanıcının sisteme giriş yaparken kullandığı kullanıcı adını tutar.
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        // Kullanıcıya atanmış tüm rol adlarını tutar; listede "Admin, User" şeklinde gösterilir.
        [Display(Name = "Roller")]
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
