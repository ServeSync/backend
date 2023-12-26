using ServeSync.Application.Common;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetStatisticQueryHandler : IQueryHandler<GetStatisticQuery, StatisticDto>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _organizationRepository;
    private readonly IBasicReadOnlyRepository<Proof, Guid> _proofRepository;
    private readonly ISpecificationService _specificationService;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUser _currentUser;
    
    public GetStatisticQueryHandler(
        IBasicReadOnlyRepository<Event, Guid> eventRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IBasicReadOnlyRepository<EventOrganization, Guid> organizationRepository,
        IBasicReadOnlyRepository<Proof, Guid> proofRepository,
        ISpecificationService specificationService,
        IIdentityService identityService,
        ICurrentUser currentUser)
    {
        _eventRepository = eventRepository;
        _studentRepository = studentRepository;
        _organizationRepository = organizationRepository;
        _proofRepository = proofRepository;
        _specificationService = specificationService;
        _identityService = identityService;
        _currentUser = currentUser;
    }
    
    public async Task<StatisticDto> Handle(GetStatisticQuery request, CancellationToken cancellationToken)
    {
        var permissions = (await _identityService.GetPermissionsForUserAsync(_currentUser.Id, _currentUser.TenantId))
            .ToList();
        
        return new StatisticDto()
        {
            TotalEvents = await _eventRepository.GetCountAsync(await _specificationService.GetEventPersonalizedSpecificationAsync()),
            TotalStudents = permissions.Contains(AppPermissions.Students.Management) 
                ? await _studentRepository.GetCountAsync() 
                : 0,
            TotalOrganizations = permissions.Contains(AppPermissions.EventOrganizations.Management) 
                ? await _organizationRepository.GetCountAsync() 
                : 0,
            TotalProof = permissions.Contains(AppPermissions.Proofs.Management) 
                ? await _proofRepository.GetCountAsync() 
                : 0,
        };
    }
}