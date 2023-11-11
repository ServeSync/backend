using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    
    private readonly IEventDomainService _eventDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<CreateEventCommandHandler> _logger;
    
    public CreateEventCommandHandler(
        IEventRepository eventRepository,
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IEventDomainService eventDomainService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<CreateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _eventDomainService = eventDomainService;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var @event = await _eventDomainService.CreateAsync(
            request.Event.Name,
            request.Event.Introduction,
            request.Event.Description,
            request.Event.ImageUrl,
            request.Event.Type,
            request.Event.StartAt,
            request.Event.EndAt,
            request.Event.ActivityId,
            request.Event.Address.FullAddress,
            request.Event.Address.Longitude,
            request.Event.Address.Latitude);
        
        foreach (var attendanceInfo in request.Event.AttendanceInfos)
        {
            _eventDomainService.AddAttendanceInfo(@event, attendanceInfo.StartAt, attendanceInfo.EndAt);
        }

        foreach (var role in request.Event.Roles)
        {
            _eventDomainService.AddRole(@event, role.Name, role.Description, role.IsNeedApprove, role.Score, role.Quantity);
        }
        
        foreach (var registrationInfo in request.Event.RegistrationInfos)
        {
            _eventDomainService.AddRegistrationInfo(@event, registrationInfo.StartAt, registrationInfo.EndAt, DateTime.UtcNow);
        }
        
        await AddOrganizationsAsync(@event, request.Event.Organizations);
        
        await _eventRepository.InsertAsync(@event);
        await _unitOfWork.CommitAsync();

        _eventDomainService.SetRepresentativeOrganization(@event, request.Event.RepresentativeOrganizationId);
        if (await _currentUser.IsAdminAsync() || await _currentUser.IsStudentAsync())
        {
            _eventDomainService.ApproveEvent(@event, DateTime.UtcNow);
        }
        
        _eventRepository.Update(@event);
        await _unitOfWork.CommitTransactionAsync(true);

        _logger.LogInformation("Created new event with id: {EventId}", @event.Id);
        
        return @event.Id;
    }

    private async Task AddOrganizationsAsync(Event @event, List<OrganizationInEventCreateDto> organizations)
    {
        var eventOrganizations = await _eventOrganizationRepository.FindByIncludedIdsAsync(organizations.Select(x => x.OrganizationId));
        var organizationRepresentatives = await _eventOrganizationContactRepository.FindByIncludedIdsAsync(organizations.SelectMany(x => x.OrganizationReps).Select(x => x.OrganizationRepId));
        
        foreach (var organization in organizations)
        {
            var eventOrganization = eventOrganizations.FirstOrDefault(x => x.Id == organization.OrganizationId);
            if (eventOrganization == null)
            {
                throw new EventOrganizationNotFoundException(organization.OrganizationId);
            }
            
            _eventDomainService.AddOrganization(@event, eventOrganization, organization.Role);
            
            AddRepresentative(@event, eventOrganization, organization, organizationRepresentatives);
        }
    }

    private void AddRepresentative(
        Event @event,
        EventOrganization eventOrganization,
        OrganizationInEventCreateDto organization, 
        IList<EventOrganizationContact> organizationRepresentatives)
    {
        foreach (var representative in organization.OrganizationReps)
        {
            var organizationRepresentative = organizationRepresentatives.FirstOrDefault(x => x.Id == representative.OrganizationRepId);
            if (organizationRepresentative == null)
            {
                throw new EventOrganizationContactNotFoundException(representative.OrganizationRepId);
            }
                
            _eventDomainService.AddRepresentative(
                @event, 
                eventOrganization,
                organizationRepresentative, 
                representative.Role);
        }
    }
}