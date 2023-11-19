using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventsQueryHandler : IQueryHandler<GetAllEventsQuery, PagedResultDto<FlatEventDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventsQueryHandler(
        ICurrentUser currentUser,
        IBasicReadOnlyRepository<Event, Guid> eventRepository, 
        IMapper mapper)
    {
        _currentUser = currentUser;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<FlatEventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var specification = await GetSpecification(request);
        
        var events = await _eventRepository.GetPagedListAsync(specification);
        var total = await _eventRepository.GetCountAsync(specification);

        return new PagedResultDto<FlatEventDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<FlatEventDto>>(events));
    }

    private async Task<IPagingAndSortingSpecification<Event, Guid>> GetSpecification(GetAllEventsQuery request)
    {
        var specification = new FilterEventSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .And(new EventByTimeFrameSpecification(request.StartDate, request.EndDate))
            .AndIf(request.EventStatus.HasValue, new EventByStatusSpecification(request.EventStatus.GetValueOrDefault(), DateTime.UtcNow))
            .AndIf(request.EventType.HasValue, new EventByTypeSpecification(request.EventType.GetValueOrDefault()));

        if (request.DefaultFilters != null && request.DefaultFilters.Any())
        {
            foreach (var filter in request.DefaultFilters)
            {
                switch (filter)
                {
                    case EventDefaultFilter.Personalized:
                        specification = specification.And(await GetPersonalizedSpecificationAsync());
                        break;
                }
            }
        }
        
        return specification;
    }
    
    private async Task<ISpecification<Event, Guid>> GetPersonalizedSpecificationAsync()
    {
        if (_currentUser.IsAuthenticated)
        {
            if (await _currentUser.IsOrganizationAsync())
            {
                return new EventByOrganizationSpecification(Guid.Parse(_currentUser.ReferenceId), _currentUser.Id);    
            }
            else if (await _currentUser.IsOrganizationContactAsync())
            {
                return new EventByOrganizationContactSpecification(Guid.Parse(_currentUser.ReferenceId));    
            }
        }
        
        return EmptySpecification<Event, Guid>.Instance;
    }
}