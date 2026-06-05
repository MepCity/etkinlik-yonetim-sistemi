using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace Services.Mail
{
    // SMTP üzerinden HTML e-posta gönderen soyut temel servis sınıfı
    public abstract class MailService : IMailService
    {
        // appsettings.json'daki MailSettings bölümüne erişim için yapılandırma nesnesi
        private readonly IConfiguration _configuration;

        // Yapılandırmayı constructor üzerinden alır
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // SMTP bağlantısı kurarak belirtilen alıcıya HTML formatlı e-posta gönderir
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:MailUser"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

           using var smtp = new SmtpClient();
           await smtp.ConnectAsync(_configuration["MailSettings:MailHost"], int.Parse(_configuration["MailSettings:MailPort"]), SecureSocketOptions.StartTls);
           await smtp.AuthenticateAsync(_configuration["MailSettings:MailUser"], _configuration["MailSettings:MailPassword"]);
           await smtp.SendAsync(email);
           await smtp.DisconnectAsync(true);
        }


    }
}
