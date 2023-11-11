using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class NewEventOrganizationCreatedDomainEventHandler : IDomainEventHandler<NewEventOrganizationCreatedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<NewEventOrganizationCreatedDomainEventHandler> _logger;
    
    public NewEventOrganizationCreatedDomainEventHandler(
        IIdentityService identityService,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<NewEventOrganizationCreatedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(NewEventOrganizationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userName = Guid.NewGuid().ToString("n").Substring(0, 8);
        var password = Guid.NewGuid().ToString("n").Substring(0, 8);
        var result = await _identityService.CreateEventOrganizationContactAsync(
            notification.Organization.Name,
            userName,
            notification.Organization.ImageUrl,
            notification.Organization.Email,
            password,
            notification.Organization.Id);

        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create identity user for event organization {EventOrganizationId}: {Message}", notification.Organization.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }

        notification.Organization.SetIdentityId(result.Data!.Id);
        _logger.LogInformation("Created new identity user {IdentityUserId} for event organization {EventOrganizationId}", notification.Organization.IdentityId, notification.Organization.Id);

        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.Organization.Email,
            Subject = "[ServeSync] Thông báo thông tin tài khoản",
            Body = _emailTemplateGenerator.GetGrantAccountToEventOrganizer(notification.Organization.Name, notification.Organization.Email, userName, password)
        });
    }
}