using System.ComponentModel.DataAnnotations;

// Bu özel doğrulama niteliği, kredi kartı numarasının Luhn algoritmasına göre geçerli olup olmadığını denetler.
// Luhn algoritması: sağdan sola çift pozisyondaki rakamlar ikiye katlanır, 9'u geçenlerden 9 çıkarılır,
// tüm rakamlar toplanır ve toplam 10'a tam bölünüyorsa kart numarası geçerlidir.
namespace EventHub.Web.Validation
{
    public class LuhnAttribute : ValidationAttribute
    {
        // Hata mesajını üst sınıfa iletir; doğrulama başarısız olduğunda kullanıcıya gösterilir.
        public LuhnAttribute() : base("Geçerli bir kart numarası girin.") { }

        // Gelen kart numarasını temizler, uzunluk ve sayısal kontrol yapar, ardından Luhn doğrulaması uygular.
        public override bool IsValid(object? value)
        {
            if (value is not string raw) return false;
            // Boşluk ve tire karakterlerini kaldırarak yalnızca rakam dizisini elde et.
            var digits = raw.Replace(" ", "").Replace("-", "");
            // Kart numarası 13-19 hane arasında olmalı ve yalnızca rakamlardan oluşmalıdır.
            if (digits.Length < 13 || digits.Length > 19 || !digits.All(char.IsDigit))
                return false;

            int sum = 0;
            bool alt = false;
            // Sağdan sola ilerleyerek Luhn toplamını hesapla.
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int n = digits[i] - '0';
                // Çift pozisyondaki rakamı ikiye katla; 9'u geçerse 9 çıkar.
                if (alt) { n *= 2; if (n > 9) n -= 9; }
                sum += n;
                alt = !alt;
            }
            // Toplam 10'a tam bölünüyorsa kart numarası geçerlidir.
            return sum % 10 == 0;
        }
    }
}
