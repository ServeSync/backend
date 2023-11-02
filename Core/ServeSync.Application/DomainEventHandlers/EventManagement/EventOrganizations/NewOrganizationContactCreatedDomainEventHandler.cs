using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventOrganizations;

public class NewOrganizationContactCreatedDomainEventHandler : IDomainEventHandler<NewOrganizationContactCreatedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<NewOrganizationContactCreatedDomainEventHandler> _logger;
    
    public NewOrganizationContactCreatedDomainEventHandler(
        IIdentityService identityService,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<NewOrganizationContactCreatedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(NewOrganizationContactCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userName = Guid.NewGuid().ToString("n").Substring(0, 8);
        var password = Guid.NewGuid().ToString("n").Substring(0, 8);
        var result = await _identityService.CreateEventOrganizationContactAsync(
            notification.Contact.Name,
            userName,
            notification.Contact.ImageUrl,
            notification.Contact.Email,
            password,
            notification.Contact.Id);

        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create identity user for event organizer {EventOrganizerId}: {Message}", notification.Contact.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }

        notification.Contact.SetIdentityId(result.Data!.Id);
        _logger.LogInformation("Created new identity user {IdentityUserId} for event organizer {EventOrganizerId}", notification.Contact.IdentityId, notification.Contact.Id);

        // _emailSender.Push(new EmailMessage()
        // {
        //     ToAddress = notification.Contact.Email,
        //     Subject = "[ServeSync] Thông báo thông tin tài khoản",
        //     Body = _emailTemplateGenerator.GetGrantAccountToEventOrganizer(notification.Contact.Name, notification.Contact.Email, userName, password)
        // });
    }
}