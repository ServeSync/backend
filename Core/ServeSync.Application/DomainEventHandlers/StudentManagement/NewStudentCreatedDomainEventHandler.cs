using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class NewStudentCreatedDomainEventHandler : IDomainEventHandler<NewStudentCreatedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly IStudentDomainService _studentDomainService;
    private readonly ILogger<NewStudentCreatedDomainEventHandler> _logger;
    
    public NewStudentCreatedDomainEventHandler(
        IIdentityService identityService, 
        IStudentDomainService studentDomainService,
        ILogger<NewStudentCreatedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _studentDomainService = studentDomainService;
        _logger = logger;
    }
    
    public async Task Handle(NewStudentCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateStudentAsync(
            @event.Student.FullName,
            @event.Student.Code,
            @event.Student.Email,
            @event.Student.Code);

        if (!result.IsSuccess)
        {
            _logger.LogError("Can not create identity user for student {StudentId}: {Message}", @event.Student.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }

        await _studentDomainService.SetIdentityAsync(@event.Student, result.Data!.Id);
        _logger.LogInformation("Created new identity user {IdentityUserId} for student {StudentId}", @event.Student.IdentityId, @event.Student.Id);
    }
}