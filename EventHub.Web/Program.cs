using BLL;
using DAL.Data;
using DAL.Data.Seeds;
using DAL.Entities;
using DAL.Repositories.Implementations;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Attachments;
using Services.Mail;
using System.Globalization;

namespace EventHub.Web
{
    // Uygulamanın giriş noktası — servis kayıtları ve HTTP istek hattı burada yapılandırılır
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Bağlantı dizesini appsettings.json'dan okur; bulunamazsa uygulama başlamadan hata fırlatır
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            // SQLite veritabanı bağlantısıyla Entity Framework DbContext'i DI konteynerine kaydeder
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));


            // ASP.NET Identity sistemini IdentityUser ve IdentityRole tipleriyle kaydeder; varsayılan UI sayfaları da eklenir
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();

            // Razor Pages desteğini (Identity UI gibi sayfalar için) DI konteynerine ekler
            builder.Services.AddRazorPages();

            // Veritabanına başlangıç verisi (seed) ekleyen servisi kaydeder
            builder.Services.AddScoped<DataSeeding>();
            // Etkinlik veritabanı işlemleri için generic repository implementasyonunu kaydeder
            builder.Services.AddScoped<IGenericRepository<Event>, EventRepository>();
            // Rezervasyon veritabanı işlemleri için generic repository implementasyonunu kaydeder
            builder.Services.AddScoped<IGenericRepository<Booking>, BookingRepository>();
            // Rezervasyon iş mantığını içeren servisi DI konteynerine kaydeder
            builder.Services.AddScoped<IBookingService , BookingService>();
            // Rezervasyon sonrası e-posta gönderme işlemlerini yöneten servisi kaydeder
            builder.Services.AddScoped<BookingMailService>();
            // E-posta şablonlarını oluşturan servisi kaydeder
            builder.Services.AddScoped<EmailTemplateService>();
            // Dosya (görsel) yükleme işlemlerini yöneten servisi kaydeder
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            // Etkinlik iş mantığını içeren servisi DI konteynerine kaydeder
            builder.Services.AddScoped<IEventService, EventService>();




            // Register Configuration service
            // IConfiguration örneğini Singleton olarak kaydeder; uygulama boyunca tek bir örnek kullanılır
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // MVC controller ve view desteğini DI konteynerine ekler
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Uygulama için Türkçe (tr-TR) kültür desteği yapılandırılır; tarih ve para birimi formatları buna göre ayarlanır
            var supportedCultures = new[] { new CultureInfo("tr-TR") };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            // Injecting the DataSeeding class to seed the database
            // Uygulama başlarken DI kapsamı oluşturulur ve veritabanına başlangıç verileri eşzamanlı olarak yazılır
            using var scope = app.Services.CreateScope();
            var dataSeedObject = scope.ServiceProvider.GetRequiredService<DataSeeding>();
            dataSeedObject.SeedData().GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            // Geliştirme ortamı dışında genel hata yakalayıcı ve HSTS güvenlik başlıkları etkinleştirilir
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // HTTP isteklerini HTTPS'e yönlendiren middleware'i etkinleştirir
            app.UseHttpsRedirection();
            // CSS, JS ve görsel gibi statik dosyaların wwwroot klasöründen sunulmasını sağlar
            app.UseStaticFiles();
            // İstek başına kültür/dil bilgisini belirleyen yerelleştirme middleware'ini etkinleştirir
            app.UseRequestLocalization(localizationOptions);

            // İstek yönlendirme altyapısını etkinleştirir
            app.UseRouting();

            // Kullanıcı kimlik doğrulamasını (oturum açma) işleyen middleware'i etkinleştirir
            app.UseAuthentication();
            // Kullanıcı yetkilendirmesini (rol kontrolü) işleyen middleware'i etkinleştirir
            app.UseAuthorization();

            // Kök URL'ye gelen istekleri oturum durumuna göre Login veya Home/Index sayfasına yönlendiren özel middleware
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/" && !context.User.Identity!.IsAuthenticated)
                {
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
                else if (context.Request.Path == "/" && context.User.Identity!.IsAuthenticated)
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }

                await next();
            });



            // Varsayılan MVC route şablonunu tanımlar: controller/action/id formatında URL eşleşmesi sağlar
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            // Razor Pages (Identity UI gibi) için route eşleştirmesini etkinleştirir
            app.MapRazorPages();

            app.Run();
        }
    }
}
