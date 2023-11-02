namespace ServeSync.Application.MailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string GetForForgetPassword(string callBackUrl);

    string GetCancelEvent(string eventName);
    
    string GetApproveEventRegistration(string studentName, string eventName, string eventRole, DateTime eventDate, string eventAddress);
    
    string GetRejectEventRegistration(string studentName, string eventName, string eventRole, string rejectReason);

    string GetGrantAccountToEventOrganizer(string name, string email, string username, string password);

    string GetApproveCollaborationRequest(string name, string eventName);
}