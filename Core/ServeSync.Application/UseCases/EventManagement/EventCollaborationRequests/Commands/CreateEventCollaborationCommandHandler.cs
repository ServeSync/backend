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
    private readonly IEventCollaborationRequestDomainService _eventCollaborationRequestDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEventCollaborationCommandHandler> _logger;

    public CreateEventCollaborationCommandHandler(
        IEventCollaborationRequestDomainService eventCollaborationRequestDomainService,
        IUnitOfWork unitOfWork,
        ILogger<CreateEventCollaborationCommandHandler> logger)
    {
        _eventCollaborationRequestDomainService = eventCollaborationRequestDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Guid> Handle(CreateEventCollaborationCommand request, CancellationToken cancellationToken)
    {
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