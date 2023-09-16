namespace ServeSync.Application.SeedWorks.MailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string GetForForgetPassword(string callBackUrl);
}