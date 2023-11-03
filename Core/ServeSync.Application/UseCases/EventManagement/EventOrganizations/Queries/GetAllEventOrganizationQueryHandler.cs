using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;
public class GetAllEventOrganizationQueryHandler : IQueryHandler<GetAllEventOrganizationQuery, PagedResultDto<EventOrganizationDto>>
{
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IMapper _mapper;

    public GetAllEventOrganizationQueryHandler(IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository, IMapper mapper)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<EventOrganizationDto>> Handle(GetAllEventOrganizationQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);

        var organizations = await _eventOrganizationRepository.GetPagedListAsync(specification);
        var total = await _eventOrganizationRepository.GetCountAsync(specification);

        return new PagedResultDto<EventOrganizationDto>(
            total, 
            request.Size,
            _mapper.Map<IEnumerable<EventOrganizationDto>>(organizations));
    }

    private IPagingAndSortingSpecification<EventOrganization, Guid> GetSpecification(GetAllEventOrganizationQuery request)
    {
        var specification = new FilterEventOrganizationSpecification(request.Page, request.Size, request.Sorting, request.Search);

        return specification;
    }
}
