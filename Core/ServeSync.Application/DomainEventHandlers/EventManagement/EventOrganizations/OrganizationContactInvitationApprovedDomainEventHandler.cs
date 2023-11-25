using Microsoft.Extensions.Logging;
using ServeSync.Application.Common;
using ServeSync.Application.Identity;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class OrganizationContactInvitationApprovedDomainEventHandler : IDomainEventHandler<OrganizationContactInvitationApprovedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ITenantService _tenantService;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<OrganizationContactInvitationApprovedDomainEventHandler> _logger;
    
    public OrganizationContactInvitationApprovedDomainEventHandler(
        IIdentityService identityService,
        ITenantService tenantService,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<OrganizationContactInvitationApprovedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _tenantService = tenantService;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(OrganizationContactInvitationApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var isNewIdentity = await InitIdentityAsync(notification.Contact, notification.Organization.TenantId.GetValueOrDefault());

        await AddToTenantAsync(notification.Contact, notification.Organization.TenantId.GetValueOrDefault());

        if (isNewIdentity)
        {
            _emailSender.Push(new EmailMessage()
            {
                ToAddress = notification.Contact.Email,
                Subject = "[ServeSync] Thông báo thông tin tài khoản",
                Body = _emailTemplateGenerator.GetGrantAccountToEventOrganizer(
                    notification.Contact.Name, 
                    notification.Contact.Email, 
                    notification.Contact.Email, 
                    AppConstants.DefaultPassword)
            });    
        }
    }
    
    private async Task<bool> InitIdentityAsync(EventOrganizationContact contact, Guid tenantId)
    {
        var user = await _identityService.GetByUserNameAsync(contact.Email);
        if (user != null)
        {
            _logger.LogInformation("Identity user {IdentityUserId} already exists for event organization contact {EventOrganizationContactId}", user.Id, contact.Id);
            
            if (!await _identityService.IsOrganizationContactAsync(contact.IdentityId, tenantId))
            {
                await _identityService.GrantToRoleAsync(user.Id, AppRole.EventOrganizer, tenantId);
                _logger.LogInformation("Granted identity user {IdentityUserId} to role {RoleName}", user.Id, AppRole.EventOrganizer);
            }
            
            contact.SetIdentityId(user.Id);
            return false;
        }

        var result = await _identityService.CreateEventOrganizationContactAsync(
            contact.Name,
            contact.Email,
            contact.ImageUrl,
            contact.Email,
            AppConstants.DefaultPassword,
            contact.Id,
            tenantId);

        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create identity user for event organization contact {EventOrganizationContactId}: {Message}", contact.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }

        contact.SetIdentityId(result.Data!.Id);
        _logger.LogInformation("Created new identity user {IdentityUserId} for event organization contact {EventOrganizationContactId}", contact.IdentityId, contact.Id);
        return true;
    }
    
    private async Task AddToTenantAsync(EventOrganizationContact contact, Guid tenantId)
    {
        await _tenantService.AddUserToTenantAsync(
            contact.IdentityId!,
            contact.Name,
            contact.ImageUrl,
            false,
            tenantId);
    }
}