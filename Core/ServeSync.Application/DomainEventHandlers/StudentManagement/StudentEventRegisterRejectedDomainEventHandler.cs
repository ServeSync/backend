using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentEventRegisterRejectedDomainEventHandler : IDomainEventHandler<StudentEventRegisterRejectedDomainEvent>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<StudentEventRegisterRejectedDomainEventHandler> _logger;
    
    public StudentEventRegisterRejectedDomainEventHandler(
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<StudentEventRegisterRejectedDomainEventHandler> logger)
    {
        _studentRepository = studentRepository;
        _eventRepository = eventRepository;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(StudentEventRegisterRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.FindAsync(new EventByRoleSpecification(notification.EventRoleId));
        if (@event == null)
        {
            _logger.LogError("Event with role id {EventRoleId} was not found!", notification.EventRoleId);
            return;
        }

        var role = @event.Roles.First(x => x.Id == notification.EventRoleId);
        
        var student = await _studentRepository.FindByIdAsync(notification.StudentId);
        if (student == null)
        {
            _logger.LogError("Student with id {StudentId} was not found!", notification.StudentId);
            return;
        }
        
        _emailSender.Push(new EmailMessage()
        {
            Subject = "[ServeSync] Thông báo đăng ký sự kiện",
            ToAddress = student.Email,
            Body = _emailTemplateGenerator.GetRejectEventRegistration(student.FullName, @event.Name, role.Name, notification.RejectReason)
        });
        
        _logger.LogInformation("Sent email to {Email} about rejected event register {EventId}", student.Email, @event.Id);
    }
}