using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Validation
{
    public class LuhnAttribute : ValidationAttribute
    {
        public LuhnAttribute() : base("Geçerli bir kart numarası girin.") { }

        public override bool IsValid(object? value)
        {
            if (value is not string raw) return false;
            var digits = raw.Replace(" ", "").Replace("-", "");
            if (digits.Length < 13 || digits.Length > 19 || !digits.All(char.IsDigit))
                return false;

            int sum = 0;
            bool alt = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int n = digits[i] - '0';
                if (alt) { n *= 2; if (n > 9) n -= 9; }
                sum += n;
                alt = !alt;
            }
            return sum % 10 == 0;
        }
    }
}
