using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;
public class CreateEventCollaborationCommandHandler : ICommandHandler<CreateEventCollaborationCommand, Guid>
{
    private readonly IEventCollaborationRequestRepository _eventCollaborationRequestRepository;
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    private readonly IEventCollaborationRequestDomainService _eventCollaborationRequestDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEventCollaborationCommandHandler> _logger;

    public CreateEventCollaborationCommandHandler(
        IEventCollaborationRequestRepository eventCollaborationRequestRepository,
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IEventCollaborationRequestDomainService eventCollaborationRequestDomainService,
        IUnitOfWork unitOfWork,
        ILogger<CreateEventCollaborationCommandHandler> logger)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _eventCollaborationRequestDomainService = eventCollaborationRequestDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Guid> Handle(CreateEventCollaborationCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        var eventCollaborationRequest = await _eventCollaborationRequestDomainService.CreateAsync(
            request.EventCollaborationRequest.Name,
            request.EventCollaborationRequest.Introduction,
            request.EventCollaborationRequest.Description,
            request.EventCollaborationRequest.Capacity,
            request.EventCollaborationRequest.ImageUrl,
            request.EventCollaborationRequest.StartAt,
            request.EventCollaborationRequest.EndAt,
            request.EventCollaborationRequest.EventType,
            request.EventCollaborationRequest.ActivityId,
            request.EventCollaborationRequest.Address.FullAddress,
            request.EventCollaborationRequest.Address.Longitude,
            request.EventCollaborationRequest.Address.Latitude,
            request.EventCollaborationRequest.EventOrganizationInfo.Name,
            request.EventCollaborationRequest.EventOrganizationInfo.Description,
            request.EventCollaborationRequest.EventOrganizationInfo.Email,
            request.EventCollaborationRequest.EventOrganizationInfo.PhoneNumber,
            request.EventCollaborationRequest.EventOrganizationInfo.Address,
            request.EventCollaborationRequest.EventOrganizationInfo.ImageUrl,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Name,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Email,
            request.EventCollaborationRequest.EventOrganizationContactInfo.PhoneNumber,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Gender,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Address,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Birth,
            request.EventCollaborationRequest.EventOrganizationContactInfo.Position,
            request.EventCollaborationRequest.EventOrganizationContactInfo.ImageUrl);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Created new event collaboration request with id: {EventCollaborationRequestId}", eventCollaborationRequest.Id);

        return eventCollaborationRequest.Id;
    }
}