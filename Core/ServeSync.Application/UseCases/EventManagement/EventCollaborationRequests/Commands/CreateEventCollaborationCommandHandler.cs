using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
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

        var eventCollaborationCreateRequest = await _eventCollaborationRequestDomainService.CreateAsync(
            request.EventCollaborationCreateRequest.Name,
            request.EventCollaborationCreateRequest.Introduction,
            request.EventCollaborationCreateRequest.Description,
            request.EventCollaborationCreateRequest.Capacity,
            request.EventCollaborationCreateRequest.ImageUrl,
            request.EventCollaborationCreateRequest.StartAt,
            request.EventCollaborationCreateRequest.EndAt,
            request.EventCollaborationCreateRequest.EventType,
            request.EventCollaborationCreateRequest.ActivityId,
            request.EventCollaborationCreateRequest.Address.FullAddress,
            request.EventCollaborationCreateRequest.Address.Longitude,
            request.EventCollaborationCreateRequest.Address.Latitude,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.Name,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.Description,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.Email,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.PhoneNumber,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.Address,
            request.EventCollaborationCreateRequest.EventOrganizationInfo.ImageUrl,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Name,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Email,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.PhoneNumber,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Gender,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Address,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Birth,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.Position,
            request.EventCollaborationCreateRequest.EventOrganizationContactInfo.ImageUrl);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Created new event collaboration request with id: {EventCollaborationRequestId}", eventCollaborationCreateRequest.Id);

        return eventCollaborationCreateRequest.Id;
    }
}