namespace ServeSync.Application.MailSender.Interfaces;

public interface IEmailSender
{
    Task SendAsync(EmailMessage emailMessage);
    
    void Push(EmailMessage emailMessage);
}