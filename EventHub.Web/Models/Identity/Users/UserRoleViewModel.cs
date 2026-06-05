using EventHub.Web.Models.Identity.Roles;
using System.ComponentModel.DataAnnotations;

// Bu ViewModel, bir kullanıcının mevcut rollerini yönetmek için kullanıcı bilgisi ve rol listesini birlikte taşır.
namespace EventHub.Web.Models.Identity.Users
{
    public class UserRoleViewModel
    {
        // Rolü güncellenecek kullanıcının Identity sistemindeki benzersiz kimliğini tutar.
        [Display(Name = "Kullanıcı Kimliği")]
        public string UserId { get; set; } = string.Empty;

        // Yönetim arayüzünde gösterilecek kullanıcı adını tutar.
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        // Sistemdeki tüm rollerin listesini tutar; her rol IsSelected ile o kullanıcıya atanıp atanmadığını gösterir.
        public List<UpdateRoleViewModel> Roles { get; set; } = [];
    }
}
