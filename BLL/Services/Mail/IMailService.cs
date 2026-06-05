using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    // E-posta gönderme işlemini tanımlayan servis arayüzü
    public interface IMailService
    {
        // Belirtilen alıcıya verilen konu ve gövdeyle e-posta gönderir
        Task SendEmailAsync(string to, string eventName, string eventDate);
    }
}
