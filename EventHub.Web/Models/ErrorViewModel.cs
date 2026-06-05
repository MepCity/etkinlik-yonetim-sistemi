// Bu ViewModel, hata sayfasında (Error.cshtml) gösterilecek tanılama bilgilerini taşır.
namespace EventHub.Web.Models
{
    public class ErrorViewModel
    {
        // Sunucunun her istek için ürettiği benzersiz takip kimliğini tutar; hata ayıklamada kullanılır.
        public string? RequestId { get; set; }

        // RequestId dolu ise true döndürür; görünümün kimliği koşullu olarak göstermesini sağlar.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
