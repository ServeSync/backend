using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class SendMailCancelledEventToStudentBackGroundJobHandler : IBackGroundJobHandler<SendMailCancelledEventToStudentBackGroundJob>
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<SendMailCancelledEventToStudentBackGroundJob> _logger;

    public SendMailCancelledEventToStudentBackGroundJobHandler(
        IEventDomainService eventDomainService,
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
        foreach (var e in job.Emails)
        {
            _emailSender.Push(new EmailMessage()
            {
                ToAddress = e,
                Subject = "[ServeSync] Thông báo hủy bỏ sự kiện",
                Body = _emailTemplateGenerator.GetCancelEvent(job.EventName)
            });
            _logger.LogInformation("Send mail to {Email} about cancel event {EventName}", e, job.EventName);
        }
        await Task.CompletedTask;
    }
}