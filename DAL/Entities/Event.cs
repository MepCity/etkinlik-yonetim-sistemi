using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    // Sistemdeki bir etkinliği temsil eden varlık sınıfı
    public class Event
    {
        // Etkinliğin benzersiz kimliği
        public int Id { get; set; }

        [Display(Name = "Etkinlik Adı")]
        // Etkinliğin adı
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        // Etkinliğe ait açıklama metni
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Başlangıç Tarihi")]
        // Etkinliğin başlangıç tarihi ve saati
        public DateTime StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        // Etkinliğin bitiş tarihi ve saati
        public DateTime EndDate { get; set; }

        [Display(Name = "Konum")]
        // Etkinliğin gerçekleşeceği konum
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Fiyat")]
        // Etkinlik bilet fiyatı
        public decimal Price { get; set; }

        [Display(Name = "Kontenjan")]
        // Etkinliğe katılabilecek maksimum kişi sayısı
        public int Capacity { get; set; }

        [Display(Name = "Kategori")]
        // Etkinliğin ait olduğu kategori
        public Category Category { get; set; }

        // Etkinliğe ait görsel URL'i (opsiyonel)
        public string? ImageUrl { get; set; }

        // Etkinliğe yapılmış rezervasyonların koleksiyonu
        public ICollection<Booking> Bookings { get; set; } = [];

        // Ödenmiş rezervasyonlar düşüldükten sonra kalan boş koltuk sayısı
        public int TicketsAvailable => Capacity - Bookings?.Where(b => b.IsPaid).Sum(b => b.Quantity) ?? 0 ;

        // Etkinlik kaydının oluşturulma tarihi
        public DateTime CreatedAt { get; set; }

        // Etkinlik kaydının son güncellenme tarihi
        public DateTime UpdatedAt { get; set; }
    }
}
