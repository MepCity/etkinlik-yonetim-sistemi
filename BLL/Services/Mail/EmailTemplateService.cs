using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    // HTML e-posta şablonlarını diskten yükleyen ve yer tutucuları değiştiren yardımcı servis sınıfı
    public class EmailTemplateService
    {
        // Belirtilen şablon adına göre BLL/EmailTemplates klasöründen HTML dosyasını okur ve içeriğini döndürür
        public string LoadTemplate(string templateName)
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName
                ?? throw new DirectoryNotFoundException("Solution directory could not be resolved.");
            var templatePath = Path.Combine(directory, "BLL", "EmailTemplates", $"{templateName}.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template {templateName} not found.");

            return File.ReadAllText(templatePath);
        }


        // Şablon metnindeki {{Anahtar}} formatındaki yer tutucuları verilen sözlükteki değerlerle değiştirir
        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }
            return template;
        }

    }
}
