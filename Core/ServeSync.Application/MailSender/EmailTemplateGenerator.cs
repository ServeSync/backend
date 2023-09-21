using Microsoft.Extensions.Hosting;
using ServeSync.Application.MailSender.Interfaces;

namespace ServeSync.Application.MailSender;

public class EmailTemplateGenerator : IEmailTemplateGenerator
{
    private readonly IHostEnvironment _env;
    
    public EmailTemplateGenerator(IHostEnvironment env)
    {
        _env = env;
    }
    
    public string GetForForgetPassword(string callBackUrl)
    {
        var template = GetTemplate("ForgetPassword");

        return template.Replace("{{CallBackUrl}}", callBackUrl);
    }

    private string GetTemplate(string templateName)
    {
        var pathToFile = GetTemplatePath(templateName);
        
        using var reader = System.IO.File.OpenText(pathToFile);
        return reader.ReadToEnd();
    }

    private string GetTemplatePath(string templateName)
    {
        return $"{_env.ContentRootPath}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}EmailTemplates{Path.DirectorySeparatorChar}{templateName}.html";
    }
}