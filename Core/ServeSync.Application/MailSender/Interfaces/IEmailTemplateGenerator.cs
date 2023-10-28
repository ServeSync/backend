namespace ServeSync.Application.MailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string GetForForgetPassword(string callBackUrl);

    string GetCancelEvent(string eventName);
    
    string GetApproveEventRegister(string studentName, string eventName, string eventRole, DateTime eventDate, string eventAddress);
}