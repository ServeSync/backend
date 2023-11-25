using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class ApproveEventCollaborationRequestCommandHandler : ICommandHandler<ApproveEventCollaborationRequestCommand, Guid>
{
    private readonly IEventCollaborationRequestRepository _eventCollaborationRequestRepository;
    private readonly IEventCollaborationRequestDomainService _eventCollaborationRequestDomainService;
    private readonly ILogger<ApproveEventCollaborationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public ApproveEventCollaborationRequestCommandHandler(
        IEventCollaborationRequestRepository eventCollaborationRequestRepository, 
        IEventCollaborationRequestDomainService eventCollaborationRequestDomainService,
        IUnitOfWork unitOfWork,
        ILogger<ApproveEventCollaborationRequestCommandHandler> logger)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _eventCollaborationRequestDomainService = eventCollaborationRequestDomainService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(ApproveEventCollaborationRequestCommand request, CancellationToken cancellationToken)
    {
        var eventCollaborationRequest = await _eventCollaborationRequestRepository.FindByIdAsync(request.Id);
        if (eventCollaborationRequest == null)
        {
            throw new EventCollaborationRequestNotFoundException(request.Id);
        }

        await _unitOfWork.BeginTransactionAsync();
        
        _eventCollaborationRequestDomainService.Approve(eventCollaborationRequest, DateTime.UtcNow);
        _eventCollaborationRequestRepository.Update(eventCollaborationRequest);
        
        await _unitOfWork.CommitTransactionAsync(true);
        
        _logger.LogInformation("Event collaboration request {Id} approved successfully!", request.Id);
        return eventCollaborationRequest.EventId!.Value;
    }
}