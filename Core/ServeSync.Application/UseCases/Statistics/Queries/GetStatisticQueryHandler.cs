using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetStatisticQueryHandler : IQueryHandler<GetStatisticQuery, StatisticDto>
{
    private readonly IReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IReadOnlyRepository<EventOrganization, Guid> _organizationRepository;
    private readonly IReadOnlyRepository<Proof, Guid> _proofRepository;
    
    public GetStatisticQueryHandler(
        IReadOnlyRepository<Event, Guid> eventRepository,
        IReadOnlyRepository<Student, Guid> studentRepository,
        IReadOnlyRepository<EventOrganization, Guid> organizationRepository,
        IReadOnlyRepository<Proof, Guid> proofRepository)
    {
        _eventRepository = eventRepository;
        _studentRepository = studentRepository;
        _organizationRepository = organizationRepository;
        _proofRepository = proofRepository;
    }
    
    public async Task<StatisticDto> Handle(GetStatisticQuery request, CancellationToken cancellationToken)
    {
        return new StatisticDto()
        {
            TotalEvents = await _eventRepository.GetCountAsync(),
            TotalStudents = await _studentRepository.GetCountAsync(),
            TotalOrganizations = await _organizationRepository.GetCountAsync(),
            TotalProof = await _proofRepository.GetCountAsync()
        };
    }
}