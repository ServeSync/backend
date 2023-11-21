using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class UpdateEventOrganizationContactCommandHandler : ICommandHandler<UpdateEventOrganizationContactCommand>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventOrganizationContactCommandHandler> _logger;
    
    public UpdateEventOrganizationContactCommandHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<UpdateEventOrganizationContactCommandHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateEventOrganizationContactCommand request, CancellationToken cancellationToken)
    {
        var eventOrganization = await _eventOrganizationRepository.FindByIdAsync(request.OrganizationId);
        if (eventOrganization == null)
        {
            throw new EventOrganizationNotFoundException(request.OrganizationId);
        }
        
        await _eventOrganizationDomainService.UpdateContactAsync(
            eventOrganization, 
            request.OrganizationContactId, 
            request.Name,
            request.PhoneNumber,
            request.ImageUrl,
            request.Gender,
            request.Birth,
            request.Address,
            request.Position);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Updated event organization contact information with id '{EventOrganizationContactId}'", request.OrganizationContactId);
    }
}