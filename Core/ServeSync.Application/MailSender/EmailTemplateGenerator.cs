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

    public string GetApproveEventRegistration(
        string studentName, 
        string eventName, 
        string eventRole, 
        DateTime eventDate,
        string eventAddress)
    {
        var template = GetTemplate("ApproveEventRegistration")
            .Replace("{{StudentName}}", studentName)
            .Replace("{{EventName}}", eventName)
            .Replace("{{Role}}", eventRole)
            .Replace("{{EventDate}}", eventDate.ToString("hh:mm dd/MM/yyyy"))
            .Replace("{{EventAddress}}", eventAddress);

        return template;
    }

    public string GetRejectEventRegistration(string studentName, string eventName, string eventRole, string rejectReason)
    {
        var template = GetTemplate("RejectEventRegistration")
            .Replace("{{StudentName}}", studentName)
            .Replace("{{EventName}}", eventName)
            .Replace("{{EventRole}}", eventRole)
            .Replace("{{RejectReason}}", rejectReason);

        return template;
    }

    public string GetGrantAccountToEventOrganizer(string name, string email, string username, string password)
    {
        var template = GetTemplate("GrantAccountToEventOrganizer")
            .Replace("{{FullName}}", name)
            .Replace("{{Email}}", email)
            .Replace("{{UserName}}", username)
            .Replace("{{Password}}", password);

        return template;
    }

    public string GetApproveCollaborationRequest(string name, string eventName)
    {
        var template = GetTemplate("ApproveCollaborationRequest")
            .Replace("{{FullName}}", name)
            .Replace("{{EventName}}", eventName);

        return template;
    }

    public string GetRejectCollaborationRequest(string name, string eventName)
    {
        var template = GetTemplate("RejectCollaborationRequest")
            .Replace("{{FullName}}", name)
            .Replace("{{EventName}}", eventName);

        return template;
    }

    public string GetRejectEvent(string name, string eventName)
    {
        var template = GetTemplate("RejectEvent")
            .Replace("{{FullName}}", name)
            .Replace("{{EventName}}", eventName);

        return template;
    }

    public string GetOrganizationInvitation(string name, string approveUrlCallBack, string rejectUrlCallBack)
    {
        var template = GetTemplate("OrganizationInvitation")
            .Replace("{{FullName}}", name)
            .Replace("{{ApproveUrlCallBack}}", approveUrlCallBack)
            .Replace("{{RejectUrlCallBack}}", rejectUrlCallBack);
        
        return template;
    }

    public string GetRejectProof(string studentName, string proofName, string rejectReason)
    {
        var template = GetTemplate("RejectProof")
            .Replace("{{StudentName}}", studentName)
            .Replace("{{ProofName}}", proofName)
            .Replace("{{RejectReason}}", rejectReason);

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