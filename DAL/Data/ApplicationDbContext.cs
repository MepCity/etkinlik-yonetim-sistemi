using DAL.Data.Configurations;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    // Uygulamanın veritabanı bağlamı; Entity Framework Core ile tablo erişimini ve yapılandırmayı yönetir
    public class ApplicationDbContext : IdentityDbContext
    {
        // Veritabanı bağlantı seçeneklerini alarak bağlamı başlatan yapıcı metot
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Etkinlikler tablosuna erişim sağlayan DbSet
        public DbSet<Event> Events { get; set; }

        // Rezervasyonlar tablosuna erişim sağlayan DbSet
        public DbSet<Booking> Bookings { get; set; }

        // Model oluşturulurken entity yapılandırmalarını ve ilişki kurallarını uygulayan metot
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Applying Event entity configuration
            builder.ApplyConfiguration(new EventConfigurations());

            // Booking ile kullanıcı arasındaki bire-çok ilişkiyi tanımlar; kullanıcı silindiğinde rezervasyon silinmez
            builder.Entity<Booking>()
                .HasOne(booking => booking.User)
                .WithMany()
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
