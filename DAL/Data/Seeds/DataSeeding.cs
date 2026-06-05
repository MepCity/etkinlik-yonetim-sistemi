using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace DAL.Data.Seeds
{
    // Uygulama ilk çalıştığında veritabanına örnek etkinlik, rol ve admin kullanıcısı ekleyen tohum veri sınıfı
    public class DataSeeding(ApplicationDbContext applicationDbContext,
                       UserManager<IdentityUser> userManager)
    {
        // Veritabanı bağlamına erişim sağlayan alan
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        // Kullanıcı oluşturma ve rol atama işlemleri için kimlik yöneticisi alanı
        private readonly UserManager<IdentityUser> _userManager = userManager;

        // Bekleyen migration'ları uygular, etkinlik/rol/admin verisi yoksa oluşturur
        public async Task SeedData()
        {
            // Checking if there are any pending migrations, migrating if any are found
            if (_applicationDbContext.Database.GetPendingMigrations().Any())
            {
                _applicationDbContext.Database.Migrate();
            }


            // Etkinlik tablosu boşsa örnek etkinlikleri veritabanına ekler
            if (!_applicationDbContext.Events.Any())
            {
                var events = new List<Event>
                {
                    new() {
                        Name = "Konser",
                        Description = "Popüler grupların sahne aldığı canlı konser.",
                        StartDate = DateTime.Now.AddDays(30),
                        EndDate = DateTime.Now.AddDays(30).AddHours(3),
                        Location = "Şehir Stadyumu",
                        Category = Category.Sport,
                        Price = 50.00m,
                        Capacity = 1000,
                    },
                    new() {
                        Name = "Sanat Sergisi",
                        Description = "Yerel sanatçıların eserlerini sergileyen etkinlik.",
                        StartDate = DateTime.Now.AddDays(60),
                        EndDate = DateTime.Now.AddDays(60).AddHours(5),
                        Location = "Sanat Galerisi",
                        Category = Category.Art,
                        Price = 20.00m,
                        Capacity = 500,

                    },
                    new() {
                        Name = "Teknoloji Konferansı",
                        Description = "Sektör liderlerinin katıldığı yıllık teknoloji konferansı.",
                        StartDate = DateTime.Now.AddDays(90),
                        EndDate = DateTime.Now.AddDays(90).AddHours(8),
                        Location = "Kongre Merkezi",
                        Category = Category.Conference,
                        Price = 150.00m,
                        Capacity = 2000,

                    },
                    new() {
                        Name = "Lezzet Festivali",
                        Description = "Dünyanın farklı mutfaklarından lezzetlerin sunulduğu festival.",
                        StartDate = DateTime.Now.AddDays(120),
                        EndDate = DateTime.Now.AddDays(120).AddHours(6),
                        Location = "Şehir Parkı",
                        Category = Category.Food,
                        Price = 10.00m,
                        Capacity = 3000,

                    },
                    new() {
                        Name = "Yardım Koşusu",
                        Description = "Yerel yardım kuruluşları için bağış toplanan 5K koşusu.",
                        StartDate = DateTime.Now.AddDays(150),
                        EndDate = DateTime.Now.AddDays(150).AddHours(4),
                        Location = "Şehir Merkezi",
                        Category = Category.Sport,
                        Price = 25.00m,
                        Capacity = 800,

                    }

                };

                _applicationDbContext.Events.AddRange(events);
                _applicationDbContext.SaveChanges();
            }

            // Rol tablosu boşsa Admin ve User rollerini veritabanına ekler
            if (!_applicationDbContext.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new() { Name = "Admin", NormalizedName = "ADMIN" },
                    new() { Name = "User", NormalizedName = "USER" }
                };
                _applicationDbContext.Roles.AddRange(roles);
                _applicationDbContext.SaveChanges();
            }

            // Varsayılan admin hesabının e-posta adresi
            const string adminEmail = "admin@eventhub.local";

            // Varsayılan admin hesabının şifresi
            const string adminPassword = "Admin123!";

            // Mevcut admin kullanıcısını e-posta adresine göre sorgular
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            // Admin kullanıcısı yoksa oluşturur ve Admin rolüne atar
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = new MailAddress(adminEmail).User,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin: {error.Description}");
                    }

                }
            }
        }
    }
}
