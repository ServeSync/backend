using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class CreateEventOrganizationCommandHandler : ICommandHandler<CreateEventOrganizationCommand, Guid>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEventOrganizationCommandHandler> _logger;

    public CreateEventOrganizationCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<CreateEventOrganizationCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Guid> Handle(CreateEventOrganizationCommand request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationDomainService.CreateAsync(
            request.EventOrganization.Name,
            request.EventOrganization.Email,
            request.EventOrganization.PhoneNumber,
            request.EventOrganization.ImageUrl,
            request.EventOrganization.Description,
            request.EventOrganization.Address);
        
        await _eventOrganizationRepository.InsertAsync(eventOrganization);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Created new event organization with id: {EventOrganizationId}", eventOrganization.Id);
        return eventOrganization.Id;
    }
}