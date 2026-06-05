using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    // Bir kullanıcının etkinliğe yaptığı rezervasyonu temsil eden varlık sınıfı
    public class Booking
    {
        // Rezervasyonun benzersiz kimliği
        public int Id { get; set; }

        // Rezervasyonun bağlı olduğu etkinliğin kimliği (yabancı anahtar)
        public int EventId { get; set; }

        // Rezervasyonun bağlı olduğu etkinlik nesnesi
        public Event Event { get; set; } = null!;

        // Rezervasyonu yapan kullanıcının kimliği (yabancı anahtar)
        public string UserId { get; set; } = string.Empty;

        // Rezervasyonu yapan kullanıcı nesnesi
        public IdentityUser User { get; set; } = null!;

        [Display(Name = "Katılımcı")]
        // Rezervasyonu yapan kişinin adı
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Adet")]
        // Rezerve edilen bilet adedi
        public int Quantity { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        // Rezervasyonun yapıldığı tarih ve saat
        public DateTime BookingDate { get; set; }

        // Rezervasyonun ödenip ödenmediğini belirten durum bayrağı
        public bool IsPaid { get; set; }
    }

}
