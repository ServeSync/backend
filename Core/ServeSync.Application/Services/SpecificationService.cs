using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.Services;

public class SpecificationService : ISpecificationService
{
    private readonly ICurrentUser _currentUser;
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    
    public SpecificationService(ICurrentUser currentUser,
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository)
    {
        _currentUser = currentUser;
        _eventOrganizationRepository = eventOrganizationRepository;
    }
    
    public async Task<ISpecification<Event, Guid>> GetEventPersonalizedSpecificationAsync()
    {
        if (_currentUser.IsAuthenticated)
        {
            if (await _currentUser.IsOrganizationAsync())
            {
                return new EventByOrganizationSpecification(Guid.Parse(_currentUser.ReferenceId), _currentUser.Id, _currentUser.TenantId);    
            }
            else if (await _currentUser.IsOrganizationContactAsync())
            {
                var organization = await _eventOrganizationRepository.FindAsync(new EventOrganizationByContactSpecification(Guid.Parse(_currentUser.ReferenceId)));
                if (organization == null)
                {
                    return EmptyFalseSpecification<Event, Guid>.Instance;
                }
                
                return new EventByOrganizationContactSpecification(
                    Guid.Parse(_currentUser.ReferenceId),
                    organization.IdentityId!,
                    organization.TenantId!.Value);    
            }
            else if (await _currentUser.IsStudentAsync())
            {
                return new EventRegisteredByStudentSpecification(Guid.Parse(_currentUser.ReferenceId));
            }
        }
        
        return EmptySpecification<Event, Guid>.Instance;
    }
}