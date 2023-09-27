using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentContactInfoUpdatedDomainEventHandler : IDomainEventHandler<StudentContactInfoUpdatedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<StudentContactInfoUpdatedDomainEventHandler> _logger;
    
    public StudentContactInfoUpdatedDomainEventHandler(
        IIdentityService identityService,
        ILogger<StudentContactInfoUpdatedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }
    
    public async Task Handle(StudentContactInfoUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var result = await _identityService.UpdateAsync(@event.IdentityId, @event.FullName, @event.Email);
        if (!result.IsSuccess)
        {
            _logger.LogError("Can not update contact info of identity user for student {StudentId}: {Message}", @event.Id, result.Error);
            throw new ResourceInvalidOperationException(result.Error!, result.ErrorCode!);
        }
        
        _logger.LogInformation("Updated contact info of identity user {IdentityUserId} for student {StudentId}", @event.IdentityId, @event.Id);
    }
}