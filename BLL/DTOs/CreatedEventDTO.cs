using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    // Yeni etkinlik oluşturma formundan gelen verileri taşıyan veri transfer nesnesi
    public class CreatedEventDTO
    {
        // Etkinliğin veritabanı kimliği
        public int Id { get; set; }
        // Etkinliğin adı
        public string Name { get; set; } = string.Empty;
        // Etkinliğin açıklaması
        public string Description { get; set; } = string.Empty;
        // Etkinliğin başlangıç tarihi ve saati
        public DateTime StartDate { get; set; }
        // Etkinliğin bitiş tarihi ve saati
        public DateTime EndDate { get; set; }
        // Etkinliğin düzenleneceği konum
        public string Location { get; set; } = string.Empty;
        // Etkinlik bilet fiyatı
        public decimal Price { get; set; }
        // Etkinlik toplam kontenjanı
        public int Capacity { get; set; }
        // Etkinliğin ait olduğu kategori
        public Category Category { get; set; }
        // Etkinlik için yüklenen kapak görseli (isteğe bağlı)
        public IFormFile? Image { get; set; }
        // Etkinlik kaydının oluşturulma tarihi
        public DateTime CreatedAt { get; set; }
        // Etkinlik kaydının son güncellenme tarihi
        public DateTime UpdatedAt { get; set; }
    }
}
