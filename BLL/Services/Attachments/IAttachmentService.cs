using Microsoft.AspNetCore.Http;

namespace Services.Attachments
{
    // Dosya yükleme ve silme işlemlerini tanımlayan servis arayüzü
    public interface IAttachmentService
    {
        // Yüklenen dosyayı belirtilen klasöre kaydeder ve erişim yolunu döndürür
        Task<string> UploadAsync(IFormFile file, string folderName);
        // Belirtilen dosya yolundaki dosyayı diskten siler; başarı durumunu döndürür
        bool Delete(string filePath);
    }
}
