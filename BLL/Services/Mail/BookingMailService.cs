using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    // Rezervasyon onay e-postası göndermekten sorumlu mail servis sınıfı
    public class BookingMailService : MailService
    {
        // Temel MailService sınıfına yapılandırmayı iletir
        public BookingMailService(IConfiguration configuration) : base(configuration){}

        // HTML şablon yükleyerek yer tutucuları doldurup kullanıcıya rezervasyon onay e-postası gönderir
        public async Task SendBookingConfirmationAsync(string to, string eventName, string eventDate)
        {
            var templateService = new EmailTemplateService();

            string template = templateService.LoadTemplate("BookConfirmationTemplate");

            var userName = to.Split('@')[0];

            var placeholders = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "EventName", eventName },
                { "EventDate", eventDate }
            };

            string body = templateService.ReplacePlaceholders(template, placeholders);
            string subject = "Your Booking Confirmation";

            await SendEmailAsync(to, subject, body);

        }

    }
}
