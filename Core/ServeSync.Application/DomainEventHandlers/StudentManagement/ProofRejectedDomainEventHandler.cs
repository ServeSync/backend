using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class ProofRejectedDomainEventHandler : IDomainEventHandler<ProofRejectedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<ProofRejectedDomainEventHandler> _logger;
    
    public ProofRejectedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<ProofRejectedDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(ProofRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventName = notification.Proof.ExternalProof?.EventName ?? notification.Proof.InternalProof!.Event!.Name;
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.Proof.Student!.Email,
            Subject = "[ServeSync] Thông báo duyệt minh chứng",
            Body = _emailTemplateGenerator.GetRejectProof(notification.Proof.Student.FullName, eventName, notification.RejectReason)
        });
    }
}