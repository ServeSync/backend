namespace ServeSync.Application.SeedWorks.MailSender;

public interface IEmailSender
{
    Task SendAsync(EmailMessage emailMessage);
}