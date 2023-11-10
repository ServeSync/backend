using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class DeleteEventOrganizationCommandHandler : ICommandHandler<DeleteEventOrganizationCommand>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteEventOrganizationCommandHandler> _logger;

    public DeleteEventOrganizationCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<DeleteEventOrganizationCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(DeleteEventOrganizationCommand request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(request.Id);
        if (eventOrganization == null)
        {
            throw new EventOrganizationNotFoundException(request.Id);
        }

        await _eventOrganizationDomainService.DeleteAsync(eventOrganization);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Event organization with id {EventOrganizationId} was deleted!", request.Id);
    }
}