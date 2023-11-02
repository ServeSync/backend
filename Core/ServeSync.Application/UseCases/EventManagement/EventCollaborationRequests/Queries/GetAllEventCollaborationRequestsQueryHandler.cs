using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Queries;

public class GetAllEventCollaborationRequestsQueryHandler : IQueryHandler<GetAllEventCollaborationRequestsQuery, PagedResultDto<EventCollaborationRequestDto>>
{
    private readonly IBasicReadOnlyRepository<EventCollaborationRequest, Guid> _eventCollaborationRequestRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventCollaborationRequestsQueryHandler(IBasicReadOnlyRepository<EventCollaborationRequest, Guid> eventCollaborationRequestRepository, IMapper mapper)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<EventCollaborationRequestDto>> Handle(GetAllEventCollaborationRequestsQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);
        
        var eventCollaborationRequests = await _eventCollaborationRequestRepository.GetPagedListAsync(specification);
        var total = await _eventCollaborationRequestRepository.GetCountAsync(specification);

        return new PagedResultDto<EventCollaborationRequestDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<EventCollaborationRequestDto>>(eventCollaborationRequests));
    }

    private IPagingAndSortingSpecification<EventCollaborationRequest, Guid> GetSpecification(GetAllEventCollaborationRequestsQuery request)
    {
        var specification = new FilterEventCollaborationRequestSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .And(new EventCollaborationRequestByTimeFrameSpecification(request.StartDate, request.EndDate))
            .AndIf(request.EventType.HasValue, new EventCollaborationRequestByTypeSpecification(request.EventType.GetValueOrDefault()))
            .AndIf(request.Status.HasValue, new EventCollaborationRequestByStatusSpecification(request.Status.GetValueOrDefault()));

        return specification;
    }
}
