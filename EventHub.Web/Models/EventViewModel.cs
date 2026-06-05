using Microsoft.AspNetCore.Mvc.Rendering;

// Bu ViewModel, etkinlik listesi ve kart bileşeninde görüntülenecek özet etkinlik verilerini taşır.
namespace EventHub.Web.Models
{
    public class EventViewModel
    {
        // Etkinliğin veritabanındaki birincil anahtarını tutar; yönlendirme (routing) işlemlerinde kullanılır.
        public int Id { get; set; }

        // Etkinliğin adını tutar; kart başlığında gösterilir.
        public string Name { get; set; } = string.Empty;

        // Etkinliğin başlangıç tarihini tutar; kart üzerinde tarih bilgisi olarak gösterilir.
        public DateTime StartDate { get; set; }

        // Etkinliğin düzenlendiği yeri tutar; kart üzerinde konum bilgisi olarak gösterilir.
        public string Location { get; set; } = string.Empty;

        // Bilet fiyatını tutar; para birimi formatıyla kart üzerinde görüntülenir.
        public decimal Price { get; set; }

        // Kalan kontenjan sayısını tutar; kaç bilet daha satılabileceğini gösterir.
        public int TicketsAvailable { get; set; }

        // Mevcut kullanıcının bu etkinlik için oluşturduğu rezervasyonun kimliğini tutar; iptal işleminde kullanılır.
        public int BookingId { get; set; }

        // Mevcut kullanıcının bu etkinliğe kayıtlı olup olmadığını tutar; buton durumunu (Katıl/İptal) belirler.
        public bool IsBookedByUser { get; set; }

        // Etkinlik görselinin sunucu üzerindeki yolunu tutar; null ise varsayılan görsel kullanılır.
        public string? ImageUrl { get; set; }
    }

}
