using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class NewPendingEventOrganizationCreatedDomainEventHandler : IDomainEventHandler<NewPendingEventOrganizationCreatedDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly IOrganizationInvitationRepository _organizationInvitationRepository;
    private readonly HttpContext? _httpContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NewPendingEventOrganizationCreatedDomainEventHandler> _logger;
    
    public NewPendingEventOrganizationCreatedDomainEventHandler(
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        IOrganizationInvitationRepository organizationInvitationRepository,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ILogger<NewPendingEventOrganizationCreatedDomainEventHandler> logger)
    {
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _organizationInvitationRepository = organizationInvitationRepository;
        _httpContext = httpContextAccessor.HttpContext;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task Handle(NewPendingEventOrganizationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var invitation = new OrganizationInvitation(notification.Organization.Id, InvitationType.Organization, Guid.NewGuid().ToString());
        await _organizationInvitationRepository.InsertAsync(invitation);

        var baseUrl = $"{_httpContext?.Request.Scheme}://{_httpContext?.Request.Host}/api";
        var approveUrlCallBack = $"{baseUrl}/{_configuration["Urls:OrganizationInvitation:Approve"]}?Code={invitation.Code}";
        var rejectUrlCallBack = $"{baseUrl}/{_configuration["Urls:OrganizationInvitation:Reject"]}?Code={invitation.Code}";
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.Organization.Email,
            Subject = "[ServeSync] Thông báo lời mời",
            Body = _emailTemplateGenerator.GetOrganizationInvitation(notification.Organization.Name, approveUrlCallBack, rejectUrlCallBack)
        });
        
        _logger.LogInformation("Sent invitation email to {Email} for event organization {EventOrganizationId}", notification.Organization.Email, notification.Organization.Id);
    }
}