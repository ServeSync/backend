using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Queries;

public class GetEventCollaborationRequestByIdQueryHandler : IQueryHandler<GetEventCollaborationRequestByIdQuery, EventCollaborationRequestDetailDto>
{
    private readonly IBasicReadOnlyRepository<EventCollaborationRequest, Guid> _eventCollaborationRequestRepository;
    private readonly IMapper _mapper;

    public GetEventCollaborationRequestByIdQueryHandler(IBasicReadOnlyRepository<EventCollaborationRequest, Guid> eventCollaborationRequestRepository,
        IMapper mapper)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _mapper = mapper;
    }
    
    public async Task<EventCollaborationRequestDetailDto> Handle(GetEventCollaborationRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var eventCollaborationRequest = await _eventCollaborationRequestRepository.FindAsync(new EventCollaborationRequestByIdSpecification(request.EventCollaborationRequestId));
        if (eventCollaborationRequest == null)
        {
            throw new EventCollaborationRequestNotFoundException(request.EventCollaborationRequestId);
        }

        return _mapper.Map<EventCollaborationRequestDetailDto>(eventCollaborationRequest);
    }
}