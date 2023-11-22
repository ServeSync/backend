using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;

public class GetAllEventOrganizationContactQueryHandler : IQueryHandler<GetAllEventOrganizationContactQuery, PagedResultDto<EventOrganizationContactDto>>
{
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventOrganizationContactQueryHandler(
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IMapper mapper)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<EventOrganizationContactDto>> Handle(GetAllEventOrganizationContactQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventOrganizationRepository.IsExistingAsync(request.EventOrganizationId))
        {
            throw new EventOrganizationNotFoundException(request.EventOrganizationId);
        }

        var specification = GetSpecification(request);
        var activities = await _eventOrganizationContactRepository.GetPagedListAsync(specification);
        var total = await _eventOrganizationContactRepository.GetCountAsync(specification);

        return new PagedResultDto<EventOrganizationContactDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<EventOrganizationContactDto>>(activities));
    }
    
    private IPagingAndSortingSpecification<EventOrganizationContact, Guid> GetSpecification(GetAllEventOrganizationContactQuery request)
    {
        var specification = new FilterEventOrganizationContactSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .And(new EventOrganizationContactByOrganizationSpecification(request.EventOrganizationId))
            .AndIf(request.Status.HasValue, new EventOrganizationContactByStatusSpecification(request.Status.GetValueOrDefault()));

        return specification;
    }
}