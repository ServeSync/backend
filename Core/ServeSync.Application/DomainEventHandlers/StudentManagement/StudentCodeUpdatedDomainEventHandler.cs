using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentCodeUpdatedDomainEventHandler : IDomainEventHandler<StudentCodeUpdatedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<StudentCodeUpdatedDomainEventHandler> _logger;
    
    public StudentCodeUpdatedDomainEventHandler(
        IIdentityService identityService,
        ILogger<StudentCodeUpdatedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }
    
    public async Task Handle(StudentCodeUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var result = await _identityService.UpdateUserNameAsync(@event.IdentityId, @event.Code);
        if (!result.IsSuccess)
        {
            _logger.LogError("Can not update username of identity user for student {StudentId}: {Message}", @event.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }
        
        _logger.LogInformation("Updated username of identity user {IdentityUserId} for student {StudentId}", @event.IdentityId, @event.Id);
    }
}