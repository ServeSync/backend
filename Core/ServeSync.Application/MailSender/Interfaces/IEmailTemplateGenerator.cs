namespace ServeSync.Application.MailSender.Interfaces;

public interface IEmailTemplateGenerator
{
    string GetForForgetPassword(string callBackUrl);
}