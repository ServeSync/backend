using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

namespace ServeSync.Application.DomainEventHandlers.StudentManagement;

public class StudentDeletedDomainEventHandler : IDomainEventHandler<StudentDeletedDomainEvent>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<StudentDeletedDomainEventHandler> _logger;
    
    public StudentDeletedDomainEventHandler(
        IIdentityService identityService,
        ILogger<StudentDeletedDomainEventHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }
    
    public async Task Handle(StudentDeletedDomainEvent @event, CancellationToken cancellationToken)
    {
        var result = await _identityService.DeleteAsync(@event.IdentityId);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Identity with id {IdentityId} was deleted!", @event.IdentityId);
        }
        else
        {
            _logger.LogError("Identity with id {IdentityId} was not deleted: {Message}", @event.IdentityId, result.Error);
        }
    }
}