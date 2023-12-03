using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class ProofRejectedDomainEventHandler : IDomainEventHandler<ProofRejectedDomainEvent>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<ProofRejectedDomainEventHandler> _logger;
    
    public ProofRejectedDomainEventHandler(
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<ProofRejectedDomainEventHandler> logger)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(ProofRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventName = notification.Proof.ProofType switch
        {
            ProofType.Internal => notification.Proof.InternalProof?.Event?.Name,
            ProofType.External => notification.Proof.ExternalProof?.EventName,
            ProofType.Special => notification.Proof.SpecialProof?.Title,
            _ => throw new ArgumentOutOfRangeException()
        };

        var student = await _studentRepository.FindByIdAsync(notification.Proof.StudentId);
        if (student == null)
        {
            throw new StudentNotFoundException(notification.Proof.StudentId);
        }
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = student.Email,
            Subject = "[ServeSync] Thông báo duyệt minh chứng",
            Body = _emailTemplateGenerator.GetRejectProof(student.FullName, eventName, notification.RejectReason)
        });
    }
}