using ServeSync.Application.Identity;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.Events;

public class EventRejectedDomainEventHandler : IDomainEventHandler<EventRejectedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    
    public EventRejectedDomainEventHandler(
        IIdentityService identityService,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator)
    {
        _identityService = identityService;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
    }
    
    public async Task Handle(EventRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByIdAsync(notification.Event.CreatedBy!);
        if (user == null)
        {
            return;
        }
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = user.Email,
            Subject = "[ServeSync] Thông báo duyệt sự kiện",
            Body = _emailTemplateGenerator.GetRejectEvent(user.FullName, notification.Event.Name)
        });
    }
}