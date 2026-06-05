using System.Diagnostics;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Controllers
{
    // Uygulamanın genel sayfalarını (ana sayfa, gizlilik, hata) sunan controller
    public class HomeController : Controller
    {
        // Uygulama genelinde loglama işlemleri için kullanılan logger servisi
        private readonly ILogger<HomeController> _logger;

        // Bağımlılık enjeksiyonu ile ILogger örneği constructor aracılığıyla alınır
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index — Uygulamanın ana sayfasını gösterir
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy — Gizlilik politikası sayfasını gösterir
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/IdentityIndex — Kimlik yönetimi (Identity) ana sayfasını gösterir
        public IActionResult IdentityIndex()
        {
            return View();
        }

        // GET: /Home/Error — Hata sayfasını gösterir; yanıt önbelleklenmez, her seferinde taze hata bilgisi sunulur
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
