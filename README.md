# EventHub

ASP.NET Core MVC tabanlı etkinlik yönetim ve bilet satış platformu. Kullanıcılar etkinliklere göz atıp bilet satın alabilir; yöneticiler etkinlik, kullanıcı ve rol yönetimini tek panelden yürütür.

**Canlı:** [eventhub-yasir-2026.azurewebsites.net](https://eventhub-yasir-2026.azurewebsites.net)

---

## Özellikler

**Kullanıcı**
- Kayıt / giriş
- Etkinlik listeleme ve detay görüntüleme
- Bilet satın alma (mock ödeme — Luhn doğrulama)
- Satın alınan biletleri listeleme ve iptal etme
- Kontenjanı dolu etkinliklere bilet alınamaz

**Admin**
- Etkinlik oluştur, düzenle, sil
- Etkinlik katılımcı listesi
- Kullanıcı ve rol yönetimi

---

## Teknoloji Yığını

| Katman | Teknoloji |
|--------|-----------|
| Platform | .NET 10, ASP.NET Core MVC |
| Kimlik | ASP.NET Core Identity |
| Veritabanı | Entity Framework Core 10, SQLite |
| Görünüm | Razor Views, Bootstrap 5, SB Admin |
| CI/CD | GitHub Actions → Azure App Service |
| Mimari | 3 Katmanlı — DAL / BLL / Web |

---

## Proje Yapısı

```
EventHub.sln
├── DAL/             → DbContext, Entity'ler, Repository'ler, Migration'lar, Seed
├── BLL/             → Servisler (Event, Booking, Mail, Attachment)
├── EventHub.Web/    → Controller'lar, View'lar, ViewModel'lar, Identity UI
└── EventHub.Tests/  → Unit testler
```

---

## Kurulum

```bash
# Bağımlılıkları yükle
dotnet restore EventHub.sln

# Çalıştır
dotnet run --project EventHub.Web/EventHub.Web.csproj --launch-profile http
```

İlk çalışmada migration'lar otomatik uygulanır, seed verisi oluşturulur.

Varsayılan adres: `http://localhost:5079`

---

## Varsayılan Giriş

| Rol | E-posta | Şifre |
|-----|---------|-------|
| Admin | admin@eventhub.local | Admin123! |

Yeni kullanıcılar `/Identity/Account/Register` üzerinden kayıt olabilir.

---

## Testler

```bash
dotnet test EventHub.Tests/EventHub.Tests.csproj
```

---

## Deploy

`main` branch'e push atıldığında GitHub Actions otomatik olarak Azure App Service'e deploy eder.

```bash
git push origin main  # → CI/CD tetiklenir → Azure'a deploy olur
```
