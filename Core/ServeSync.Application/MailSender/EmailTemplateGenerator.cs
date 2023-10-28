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

    public string GetCancelEvent(string eventName)
    {
        var template = GetTemplate("CancelEvent");

        return template.Replace("{{EventName}}", eventName);
    }

    public string GetApproveEventRegister(
        string studentName, 
        string eventName, 
        string eventRole, 
        DateTime eventDate,
        string eventAddress)
    {
        var template = GetTemplate("ApproveEvent")
            .Replace("{{StudentName}}", studentName)
            .Replace("{{EventName}}", eventName)
            .Replace("{{Role}}", eventRole)
            .Replace("{{EventDate}}", eventDate.ToString("hh:mm dd/MM/yyyy"))
            .Replace("{{EventAddress}}", eventAddress);

        return template;
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