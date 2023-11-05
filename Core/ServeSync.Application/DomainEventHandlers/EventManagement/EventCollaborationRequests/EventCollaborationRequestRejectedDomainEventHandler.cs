using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventCollaborationRequests;

public class EventCollaborationRequestRejectedDomainEventHandler : IDomainEventHandler<EventCollaborationRequestRejectedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<EventCollaborationRequestRejectedDomainEventHandler> _logger;
    
    public EventCollaborationRequestRejectedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<EventCollaborationRequestRejectedDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(EventCollaborationRequestRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.EventCollaborationRequest.OrganizationContact.Email,
            Subject = "[ServeSync] Thông báo không duyệt đơn hợp tác",
            Body = _emailTemplateGenerator.GetRejectCollaborationRequest(notification.EventCollaborationRequest.OrganizationContact.Name, notification.EventCollaborationRequest.Name)
        });
    }
}