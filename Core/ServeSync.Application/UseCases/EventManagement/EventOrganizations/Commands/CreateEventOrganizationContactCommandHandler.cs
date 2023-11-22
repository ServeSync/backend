using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class CreateEventOrganizationContactCommandHandler : ICommandHandler<CreateEventOrganizationContactCommand, Guid>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEventOrganizationContactCommandHandler> _logger;
    
    public CreateEventOrganizationContactCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<CreateEventOrganizationContactCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Guid> Handle(CreateEventOrganizationContactCommand request, CancellationToken cancellationToken)
    {
        var organization = await _eventOrganizationRepository.FindByIdAsync(request.OrganizationId);
        if (organization == null)
        {
            throw new EventOrganizationNotFoundException(request.OrganizationId);
        }

        var contact = _eventOrganizationDomainService.AddContact(
            organization,
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.ImageUrl,
            request.Gender,
            request.Birth,
            request.Address,
            request.Position
        );

        _eventOrganizationRepository.Update(organization);
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Created new contact for organization {OrganizationId}", request.OrganizationId);

        return contact.Id;
    }
}