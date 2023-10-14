using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllEventsQueryHandler : IQueryHandler<GetAllEventsQuery, PagedResultDto<FlatEventDto>>
{
    private readonly IBasicReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventsQueryHandler(IBasicReadOnlyRepository<Event, Guid> eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<FlatEventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);
        
        var events = await _eventRepository.GetPagedListAsync(specification);
        var total = await _eventRepository.GetCountAsync(specification);

        return new PagedResultDto<FlatEventDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<FlatEventDto>>(events));
    }

    private IPagingAndSortingSpecification<Event, Guid> GetSpecification(GetAllEventsQuery request)
    {
        var specification = new FilterEventSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .And(new EventByTimeFrameSpecification(request.StartDate, request.EndDate))
            .AndIf(request.EventStatus.HasValue, new EventByStatusSpecification(request.EventStatus.GetValueOrDefault()))
            .AndIf(request.EventType.HasValue, new EventByTypeSpecification(request.EventType.GetValueOrDefault()));

        return specification;
    }
}