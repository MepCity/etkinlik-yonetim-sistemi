using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    public class EmailTemplateService
    {
        public string LoadTemplate(string templateName)
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName
                ?? throw new DirectoryNotFoundException("Solution directory could not be resolved.");
            var templatePath = Path.Combine(directory, "BLL", "EmailTemplates", $"{templateName}.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template {templateName} not found.");

            return File.ReadAllText(templatePath);
        }


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
