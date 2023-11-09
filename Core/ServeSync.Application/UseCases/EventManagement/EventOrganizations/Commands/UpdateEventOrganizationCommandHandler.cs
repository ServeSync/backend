using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class UpdateEventOrganizationCommandHandler : ICommandHandler<UpdateEventOrganizationCommand>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventOrganizationCommandHandler> _logger;

    public UpdateEventOrganizationCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<UpdateEventOrganizationCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateEventOrganizationCommand request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(request.Id);
        if (eventOrganization == null)
        {
            throw new EventOrganizationNotFoundException(request.Id);
        }

        await _eventOrganizationDomainService.UpdateBaseInfoAsync(
            eventOrganization,
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.ImageUrl,
            request.Description,
            request.Address);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Updated event organization information with id '{EventOrganizationId}'", eventOrganization.Id);
    }
}