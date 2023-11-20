using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class DeleteEventOrganizationContactCommandHandler : ICommandHandler<DeleteEventOrganizationContactCommand>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteEventOrganizationContactCommandHandler> _logger;

    public DeleteEventOrganizationContactCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<DeleteEventOrganizationContactCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteEventOrganizationContactCommand request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(request.EventOrganizationId);
        if (eventOrganization == null)
        {
            throw new EventOrganizationNotFoundException(request.EventOrganizationId);
        }
        
        await _eventOrganizationDomainService.DeleteContactAsync(eventOrganization, request.EventOrganizationContactId);
        _eventOrganizationRepository.Update(eventOrganization);
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Event organization contact with id {EventOrganizationContactId} was deleted!", request.EventOrganizationContactId);
    }
}