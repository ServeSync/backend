using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    
    private readonly IEventDomainService _eventDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventCommandHandler> _logger;
    
    public UpdateEventCommandHandler(
        IEventRepository eventRepository, 
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IEventDomainService eventDomainService, 
        IUnitOfWork unitOfWork, 
        ILogger<UpdateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _eventDomainService = eventDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var @event = await _eventRepository.FindByIdAsync(request.Id);
        if (@event == null)
        {
            throw new EventNotFoundException(request.Id);
        }

        var dateTime = DateTime.UtcNow;

        if (request.IsUpdateBasicInfo())
        {
            await _eventDomainService.UpdateAsync(
                @event,
                request.Event.Name!,
                request.Event.Introduction!,
                request.Event.Description!,
                request.Event.ImageUrl!,
                request.Event.Type!.Value,
                request.Event.StartAt!.Value,
                request.Event.EndAt!.Value,
                request.Event.ActivityId!.Value,
                request.Event.Address!.FullAddress,
                request.Event.Address.Longitude,
                request.Event.Address.Latitude,
                dateTime);    
        }
        
        UpdateEventRoles(@event, request, dateTime);
        UpdateRegistrationInfos(@event, request, dateTime);
        UpdateAttendanceInfos(@event, request, dateTime);
        await UpdateOrganizationAsync(@event, request, dateTime);
        
        _eventRepository.Update(@event);
        await _unitOfWork.CommitAsync();

        if (request.Event.RepresentativeOrganizationId.HasValue && request.Event.Organizations != null)
        {
            _eventDomainService.SetRepresentativeOrganization(@event, request.Event.RepresentativeOrganizationId.Value, dateTime);    
        }
        
        await _unitOfWork.CommitTransactionAsync(true);
        
        _logger.LogInformation("Event with Id '{EventId}' has been updated!", @event.Id);
    }

    private void UpdateEventRoles(Event @event, UpdateEventCommand request, DateTime dateTime)
    {
        if (request.Event.Roles == null)
        {
            return;
        }
        
        var newRoles = request.Event.Roles.Where(x => !x.Id.HasValue).ToList();
        var updateRoles = request.Event.Roles.Where(x => x.Id.HasValue).ToList();
        var deletedRoles = @event.Roles.ExceptBy(updateRoles.Select(x => x.Id), x => x.Id).ToList();

        foreach (var role in deletedRoles)
        {
            _eventDomainService.RemoveRole(@event, role.Id, dateTime);
        }

        foreach (var role in updateRoles)
        {
            _eventDomainService.UpdateRole(@event, role.Id!.Value, role.Name, role.Description, role.IsNeedApprove, role.Score, role.Quantity, dateTime);
        }
        
        foreach (var role in newRoles)
        {
            _eventDomainService.AddRole(@event, role.Name, role.Description, role.IsNeedApprove, role.Score, role.Quantity, dateTime);
        }
    }

    private void UpdateRegistrationInfos(Event @event, UpdateEventCommand request, DateTime dateTime)
    {
        if (request.Event.RegistrationInfos == null)
        {
            return;
        }
        
        var newRegistrationInfos = request.Event.RegistrationInfos.Where(x => !x.Id.HasValue).ToList();
        var updateRegistrationInfos = request.Event.RegistrationInfos.Where(x => x.Id.HasValue).ToList();
        var deletedRegistrationInfos = @event.RegistrationInfos.ExceptBy(updateRegistrationInfos.Select(x => x.Id), x => x.Id).ToList();
        
        foreach (var registrationInfo in deletedRegistrationInfos)
        {
            _eventDomainService.RemoveRegistrationInfo(@event, registrationInfo.Id, dateTime);
        }
        
        foreach (var registrationInfo in updateRegistrationInfos)
        {
            _eventDomainService.UpdateRegistrationInfo(@event, registrationInfo.Id!.Value, registrationInfo.StartAt, registrationInfo.EndAt, dateTime);
        }

        foreach (var registrationInfo in newRegistrationInfos)
        {
            _eventDomainService.AddRegistrationInfo(@event, registrationInfo.StartAt, registrationInfo.EndAt, dateTime);
        }
    }

    private void UpdateAttendanceInfos(Event @event, UpdateEventCommand request, DateTime dateTime)
    {
        if (request.Event.AttendanceInfos == null)
        {
            return;
        }
        
        var newAttendanceInfos = request.Event.AttendanceInfos.Where(x => !x.Id.HasValue).ToList();
        var updateAttendanceInfos = request.Event.AttendanceInfos.Where(x => x.Id.HasValue).ToList();
        var deletedAttendanceInfos = @event.AttendanceInfos.ExceptBy(updateAttendanceInfos.Select(x => x.Id), x => x.Id).ToList();
        
        foreach (var attendanceInfo in deletedAttendanceInfos)
        {
            _eventDomainService.RemoveAttendanceInfo(@event, attendanceInfo.Id, dateTime);
        }
        
        foreach (var attendanceInfo in updateAttendanceInfos)
        {
            _eventDomainService.UpdateAttendanceInfo(@event, attendanceInfo.Id!.Value, attendanceInfo.StartAt, attendanceInfo.EndAt, dateTime);
        }
        
        foreach (var attendanceInfo in newAttendanceInfos)
        {
            _eventDomainService.AddAttendanceInfo(@event, attendanceInfo.StartAt, attendanceInfo.EndAt, dateTime);
        }
    }

    private async Task UpdateOrganizationAsync(Event @event, UpdateEventCommand request, DateTime dateTime)
    {
        if (request.Event.Organizations == null)
        {
            return;
        }
        
        var eventOrganizations = await _eventOrganizationRepository.FindByIncludedIdsAsync(request.Event.Organizations.Select(x => x.OrganizationId));
        var organizationRepresentatives = await _eventOrganizationContactRepository.FindByIncludedIdsAsync(request.Event.Organizations.SelectMany(x => x.OrganizationReps).Select(x => x.OrganizationRepId));
        
        var newOrganizations = request.Event.Organizations.Where(x => !x.Id.HasValue).ToList();
        var updateOrganizations = request.Event.Organizations.Where(x => x.Id.HasValue).ToList();
        var deletedOrganizations = @event.Organizations.ExceptBy(updateOrganizations.Select(x => x.Id), x => x.Id).ToList();

        foreach (var organizationInEvent in deletedOrganizations)
        {
            _eventDomainService.RemoveOrganization(@event, organizationInEvent.Id, dateTime);
        }
        
        foreach (var organizationInEvent in updateOrganizations)
        {
            var eventOrganization = eventOrganizations.FirstOrDefault(x => x.Id == organizationInEvent.OrganizationId);
            if (eventOrganization == null)
            {
                throw new EventOrganizationNotFoundException(organizationInEvent.OrganizationId);
            }

            var isUpdateOrganization = @event.Organizations.FirstOrDefault(x => x.Id == organizationInEvent.Id)?.OrganizationId != organizationInEvent.OrganizationId;
            
            _eventDomainService.UpdateOrganization(@event, organizationInEvent.Id!.Value, eventOrganization, organizationInEvent.Role, dateTime);
            
            UpdateOrganizationRepresentatives(@event, eventOrganization, organizationInEvent, organizationRepresentatives, isUpdateOrganization, dateTime);
        }
        
        foreach (var organizationInEvent in newOrganizations)
        {
            var eventOrganization = eventOrganizations.FirstOrDefault(x => x.Id == organizationInEvent.OrganizationId);
            if (eventOrganization == null)
            {
                throw new EventOrganizationNotFoundException(organizationInEvent.OrganizationId);
            }
            
            _eventDomainService.AddOrganization(@event, eventOrganization, organizationInEvent.Role, dateTime);
            AddRepresentative(@event, eventOrganization, organizationInEvent, organizationRepresentatives, dateTime);
        }
    }
    
    private void AddRepresentative(
        Event @event,
        EventOrganization eventOrganization,
        OrganizationInEventUpdateDto organization, 
        IList<EventOrganizationContact> organizationRepresentatives,
        DateTime dateTime)
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
                representative.Role,
                dateTime);
        }
    }

    private void UpdateOrganizationRepresentatives(
        Event @event,
        EventOrganization eventOrganization,
        OrganizationInEventUpdateDto organization,
        IList<EventOrganizationContact> organizationRepresentatives,
        bool isUpdateOrganization,
        DateTime dateTime)
    {
        var newRepresentatives = organization.OrganizationReps.Where(x => !x.Id.HasValue).ToList();

        if (!isUpdateOrganization)
        {
            var updateRepresentatives = organization.OrganizationReps.Where(x => x.Id.HasValue).ToList();
            var deletedRepresentatives = @event.Organizations.First(x => x.OrganizationId == eventOrganization.Id).Representatives.ExceptBy(updateRepresentatives.Select(x => x.Id), x => x.Id).ToList();
            
            foreach (var representative in deletedRepresentatives)
            {
                _eventDomainService.RemoveRepresentative(@event, eventOrganization, representative.Id, dateTime);
            }
        
            foreach (var representative in updateRepresentatives)
            {
                var organizationRepresentative = organizationRepresentatives.FirstOrDefault(x => x.Id == representative.OrganizationRepId);
                if (organizationRepresentative == null)
                {
                    throw new EventOrganizationContactNotFoundException(representative.OrganizationRepId);
                }
            
                _eventDomainService.UpdateRepresentative(@event, eventOrganization, representative.Id!.Value, organizationRepresentative, representative.Role, dateTime);
            }    
        }
        
        foreach (var representative in newRepresentatives)
        {
            var organizationRepresentative = organizationRepresentatives.FirstOrDefault(x => x.Id == representative.OrganizationRepId);
            if (organizationRepresentative == null)
            {
                throw new EventOrganizationContactNotFoundException(representative.OrganizationRepId);
            }
            
            _eventDomainService.AddRepresentative(@event, eventOrganization, organizationRepresentative, representative.Role, dateTime);
        }
    }
}