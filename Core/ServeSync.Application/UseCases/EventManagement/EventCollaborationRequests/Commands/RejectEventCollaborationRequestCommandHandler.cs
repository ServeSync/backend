using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class RejectEventCollaborationRequestCommandHandler : ICommandHandler<RejectEventCollaborationRequestCommand>
{
    private readonly IEventCollaborationRequestRepository _eventCollaborationRequestRepository;
    private readonly IEventCollaborationRequestDomainService _eventCollaborationRequestDomainService;
    private readonly ILogger<RejectEventCollaborationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public RejectEventCollaborationRequestCommandHandler(
        IEventCollaborationRequestRepository eventCollaborationRequestRepository, 
        IEventCollaborationRequestDomainService eventCollaborationRequestDomainService,
        IUnitOfWork unitOfWork,
        ILogger<RejectEventCollaborationRequestCommandHandler> logger)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _eventCollaborationRequestDomainService = eventCollaborationRequestDomainService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RejectEventCollaborationRequestCommand request, CancellationToken cancellationToken)
    {
        var eventCollaborationRequest = await _eventCollaborationRequestRepository.FindByIdAsync(request.Id);
        if (eventCollaborationRequest == null)
        {
            throw new EventCollaborationRequestNotFoundException(request.Id);
        }

        _eventCollaborationRequestDomainService.Reject(eventCollaborationRequest, DateTime.Now);
        _eventCollaborationRequestRepository.Update(eventCollaborationRequest);
        
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Event collaboration request {Id} rejected successfully!", request.Id);
    }
}