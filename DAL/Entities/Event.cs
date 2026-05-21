using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Display(Name = "Etkinlik Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Konum")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Kontenjan")]
        public int Capacity { get; set; }

        [Display(Name = "Kategori")]
        public Category Category { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<Booking> Bookings { get; set; } = [];
        public int TicketsAvailable => Capacity - Bookings?.Where(b => b.IsPaid).Sum(b => b.Quantity) ?? 0 ;
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}
