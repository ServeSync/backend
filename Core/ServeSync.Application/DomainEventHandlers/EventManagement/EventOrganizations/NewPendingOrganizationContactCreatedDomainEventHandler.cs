using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class NewPendingOrganizationContactCreatedDomainEventHandler : IDomainEventHandler<NewPendingOrganizationContactCreatedDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly IOrganizationInvitationRepository _organizationInvitationRepository;
    private readonly IConfiguration _configuration;
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly ILogger<NewPendingOrganizationContactCreatedDomainEventHandler> _logger;
    
    public NewPendingOrganizationContactCreatedDomainEventHandler(
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        IOrganizationInvitationRepository organizationInvitationRepository,
        IConfiguration configuration,
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        ILogger<NewPendingOrganizationContactCreatedDomainEventHandler> logger)
    {
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _organizationInvitationRepository = organizationInvitationRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task Handle(NewPendingOrganizationContactCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(notification.Contact.EventOrganizationId);
        var invitation = new OrganizationInvitation(notification.Contact.Id, InvitationType.Contact, Guid.NewGuid().ToString());
        await _organizationInvitationRepository.InsertAsync(invitation);

        var baseUrl = $"{_configuration["Urls:Api"]}/api";
        var approveUrlCallBack = $"{baseUrl}/{_configuration["Urls:OrganizationInvitation:Approve"]}?Code={invitation.Code}";
        var rejectUrlCallBack = $"{baseUrl}/{_configuration["Urls:OrganizationInvitation:Reject"]}?Code={invitation.Code}";
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.Contact.Email,
            Subject = "[ServeSync] Thông báo lời mời",
            Body = _emailTemplateGenerator.GetOrganizationContactInvitation(notification.Contact.Name, eventOrganization!.Name, approveUrlCallBack, rejectUrlCallBack)
        });
        
        _logger.LogInformation("Sent invitation email to {Email} for event organization contact {EventOrganizationContactId}", notification.Contact.Email, notification.Contact.Id);
    }
}