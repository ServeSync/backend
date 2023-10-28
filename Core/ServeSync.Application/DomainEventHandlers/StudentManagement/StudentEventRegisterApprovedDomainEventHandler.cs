using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentEventRegisterApprovedDomainEventHandler : IDomainEventHandler<StudentEventRegisterApprovedDomainEvent>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<StudentEventRegisterApprovedDomainEventHandler> _logger;
    
    public StudentEventRegisterApprovedDomainEventHandler(
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<StudentEventRegisterApprovedDomainEventHandler> logger)
    {
        _eventRepository = eventRepository;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(StudentEventRegisterApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.FindAsync(new EventByRoleSpecification(notification.EventRoleId));
        if (@event == null)
        {
            _logger.LogError("Event with role id {EventRoleId} was not found!", notification.EventRoleId);
            return;
        }

        var role = @event.Roles.First(x => x.Id == notification.EventRoleId);
        
        _emailSender.Push(new EmailMessage()
        {
            Subject = "[ServeSync] Thông báo đăng ký sự kiện",
            ToAddress = notification.Student.Email,
            Body = _emailTemplateGenerator.GetApproveEventRegister(notification.Student.FullName, @event.Name, role.Name, @event.StartAt, @event.Address.FullAddress)
        });
        
        _logger.LogInformation("Sent email to {Email} about approved event register {EventId}", notification.Student.Email, @event.Id);
    }
}