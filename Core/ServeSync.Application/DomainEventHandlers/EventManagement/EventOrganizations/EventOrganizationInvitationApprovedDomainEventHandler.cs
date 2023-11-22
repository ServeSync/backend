using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class EventOrganizationInvitationApprovedDomainEventHandler : IDomainEventHandler<EventOrganizationInvitationApprovedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ITenantService _tenantService;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<EventOrganizationInvitationApprovedDomainEventHandler> _logger;
    
    public EventOrganizationInvitationApprovedDomainEventHandler(
        IIdentityService identityService,
        ITenantService tenantService,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<EventOrganizationInvitationApprovedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _tenantService = tenantService;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(EventOrganizationInvitationApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await InitTenantAsync(notification.Organization);
        
        var password = await InitIdentityAsync(notification.Organization);

        await InitTenantOwnerAsync(notification.Organization);
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.Organization.Email,
            Subject = "[ServeSync] Thông báo thông tin tài khoản",
            Body = _emailTemplateGenerator.GetGrantAccountToEventOrganizer(notification.Organization.Name, notification.Organization.Email, notification.Organization.Email, password)
        });
    }
    
    private async Task InitTenantAsync(EventOrganization organization)
    {
        var result = await _tenantService.CreateAsync(organization.Name, organization.ImageUrl);
        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create tenant for event organization {EventOrganizationId}: {Message}", organization.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }
        
        organization.SetTenantId(result.Data!.Id);
        _logger.LogInformation("Created new tenant {TenantId} for event organization {EventOrganizationId}", result.Data!.Id, organization.Id);
    }

    private async Task<string> InitIdentityAsync(EventOrganization organization)
    {
        var password = "servesync@123";
        var result = await _identityService.CreateEventOrganizationAsync(
            organization.Name,
            organization.Email,
            organization.ImageUrl,
            organization.Email,
            password,
            organization.Id);

        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create identity user for event organization {EventOrganizationId}: {Message}", organization.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }

        organization.SetIdentityId(result.Data!.Id);
        _logger.LogInformation("Created new identity user {IdentityUserId} for event organization {EventOrganizationId}", organization.IdentityId, organization.Id);
        return password;
    }
    
    private async Task InitTenantOwnerAsync(EventOrganization organization)
    {
        await _tenantService.AddUserToTenantAsync(
            organization.IdentityId!,
            organization.Name,
            organization.ImageUrl,
            true,
            organization.TenantId!.Value);
    }
}