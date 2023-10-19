using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class SendMailCancelledEventToStudentBackGroundJobHandler : IBackGroundJobHandler<SendMailCancelledEventToStudentBackGroundJob>
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<SendMailCancelledEventToStudentBackGroundJob> _logger;

    public SendMailCancelledEventToStudentBackGroundJobHandler(
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<SendMailCancelledEventToStudentBackGroundJob> logger)
    {
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }

    public async Task Handle(SendMailCancelledEventToStudentBackGroundJob job, CancellationToken cancellationToken)
    {
        foreach (var email in job.Emails)
        {
            await _emailSender.SendAsync(new EmailMessage()
            {
                ToAddress = email,
                Subject = "[ServeSync] Thông báo hủy bỏ sự kiện",
                Body = _emailTemplateGenerator.GetCancelEvent(job.EventName)
            }); 
            _logger.LogInformation("Send mail to {Email} about cancel event {EventName}", email, job.EventName);
        }
    }
}