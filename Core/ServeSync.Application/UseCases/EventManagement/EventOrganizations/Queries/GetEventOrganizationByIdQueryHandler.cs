using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;

public class GetEventOrganizationByIdQueryHandler : IQueryHandler<GetEventOrganizationByIdQuery, EventOrganizationDetailDto>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IMapper _mapper;
    
    public GetEventOrganizationByIdQueryHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IMapper mapper)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _mapper = mapper;
    }
    public async Task<EventOrganizationDetailDto> Handle(GetEventOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindAsync(new EventOrganizationByIdSpecification(request.OrganizationId));
        if (eventOrganization == null)
        {
            throw new EventOrganizationNotFoundException(request.OrganizationId);
        }
        
        return _mapper.Map<EventOrganizationDetailDto>(eventOrganization);
    }
}