// Bu ViewModel, bir kullanıcıya rol atama/kaldırma ekranındaki her bir rol satırını temsil eder.
namespace EventHub.Web.Models.Identity.Roles

{
    public class UpdateRoleViewModel
    {
        // Rolün Identity sistemindeki benzersiz kimliğini tutar; güncelleme işleminde hangi rolün değiştiğini belirler.
        public string Id { get; set; } = string.Empty;

        // Ekranda gösterilen rol adını tutar (örneğin "Admin", "User").
        public string Name { get; set; } = string.Empty;

        // Bu rolün ilgili kullanıcıya atanıp atanmadığını tutar; onay kutusu (checkbox) durumunu yansıtır.
        public bool IsSelected { get; set; }
    }
}
