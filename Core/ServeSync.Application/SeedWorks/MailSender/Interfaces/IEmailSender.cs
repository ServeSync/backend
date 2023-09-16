namespace ServeSync.Application.SeedWorks.MailSender.Interfaces;

public interface IEmailSender
{
    Task SendAsync(EmailMessage emailMessage);
}