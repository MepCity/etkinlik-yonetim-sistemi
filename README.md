# EventHub — Etkinlik & Workshop Yönetim Sistemi

Bilgisayar Mühendisliği Web Programlama dersi 2. projesi kapsamında geliştirilmiş ASP.NET Core MVC tabanlı etkinlik yönetim uygulaması.

---

## Özellikler

### Kullanıcı
- Kayıt ol ve giriş yap
- Tüm etkinlikleri listele ve detaylarını incele
- Etkinliğe katıl (Join)
- Kendi katılımlarını listele ve iptal et
- Kontenjan dolu etkinliklere katılamaz

### Admin / Organizatör
- Etkinlik oluştur, düzenle, sil
- Etkinlik başına katılımcı listesini görüntüle
- Kullanıcı ve rol yönetimi

---

## Teknoloji Yığını

| Katman | Teknoloji |
|--------|-----------|
| Platform | .NET 10, ASP.NET Core MVC |
| Kimlik Doğrulama | ASP.NET Core Identity |
| Veritabanı | Entity Framework Core 10, SQLite |
| Görünüm | Razor Views, Partial Views, Bootstrap 5 |
| Mimari | 3 Katmanlı (DAL / BLL / Web) |

---

## Proje Yapısı

```
EventHub.sln
├── DAL/                  → DbContext, Entity'ler, Repository'ler, Migration'lar, Seed
├── BLL/                  → Servisler (Event, Booking, Mail, Attachment)
├── EventHub.Web/         → Controller'lar, View'lar, ViewModel'lar, Identity UI
└── EventHub.Tests/       → Unit testler
```

---

## Kurulum ve Çalıştırma

### 1. Paketleri yükle

```bash
dotnet restore EventHub.sln
```

### 2. Uygulamayı çalıştır

```bash
dotnet run --project EventHub.Web/EventHub.Web.csproj --launch-profile http
```

> Uygulama ilk çalıştığında migration'ları otomatik uygular ve veritabanını seed eder.

### 3. Tarayıcıda aç

```
http://localhost:5079
```

---

## Varsayılan Kullanıcılar

| Rol | E-posta | Şifre |
|-----|---------|-------|
| Admin | admin@eventhub.local | Admin123! |

Yeni kullanıcılar `/Identity/Account/Register` üzerinden kayıt olabilir. Kayıt olan kullanıcılar otomatik olarak `User` rolüne atanır.

---

## Seed Verisi

Uygulama ilk açılışta şunları otomatik oluşturur:

- 5 örnek etkinlik (farklı kategori ve kontenjanlarla)
- `Admin` ve `User` rolleri
- Yukarıdaki admin hesabı

---

## Testler

```bash
dotnet test EventHub.Tests/EventHub.Tests.csproj
```

---

## Kurs Gereksinimleri Karşılama Durumu

| Gereksinim | Durum |
|------------|-------|
| EF Core Code-First + DbContext + Migration | ✅ |
| ASP.NET Core Identity | ✅ |
| Role-based yetkilendirme (Admin / User) | ✅ |
| `_Layout.cshtml` ortak Header/Footer | ✅ |
| ViewModel kullanımı | ✅ |
| Data Annotations validasyon | ✅ |
| Partial View (etkinlik kartları) | ✅ |
| Kontenjan sınırı ve kontrolü | ✅ |
| Data Seeding | ✅ |
