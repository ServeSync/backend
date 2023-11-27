using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class RejectEventOrganizationInvitationCommandHandler : ICommandHandler<RejectEventOrganizationInvitationCommand>
{
    private readonly IOrganizationInvitationRepository _organizationInvitationRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectEventOrganizationInvitationCommandHandler> _logger;
    
    public RejectEventOrganizationInvitationCommandHandler(
        IOrganizationInvitationRepository organizationInvitationRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<RejectEventOrganizationInvitationCommandHandler> logger)
    {
        _organizationInvitationRepository = organizationInvitationRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(RejectEventOrganizationInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _organizationInvitationRepository.FindByCodeAsync(request.Code);
        if (invitation == null)
        {
            throw new OrganizationInvitationNotFoundException(request.Code);
        }
        
        await _unitOfWork.BeginTransactionAsync();
        
        await ProcessInvitationAsync(invitation);
        
        await _unitOfWork.CommitTransactionAsync(true);
        
        _logger.LogInformation("Approved invitation {InvitationCode}!", request.Code);
    }

    private async Task ProcessInvitationAsync(OrganizationInvitation invitation)
    {
        _eventOrganizationDomainService.ProcessInvitation(invitation);
        if (invitation.Type == InvitationType.Organization)
        {
            await RejectEventOrganizationAsync(invitation);
        }
        else
        {
            await RejectEventOrganizationContactAsync(invitation);
        }
    }

    private async Task RejectEventOrganizationAsync(OrganizationInvitation invitation)
    {
        var organization = await _eventOrganizationRepository.FindByIdAsync(invitation.ReferenceId);
        if (organization == null)
        {
            throw new EventOrganizationNotFoundException(invitation.ReferenceId);
        }

        organization.RejectInvitation();
        _eventOrganizationRepository.Update(organization);
        _logger.LogInformation("Rejected invitation {InvitationCode} for organization {OrganizationId}!", invitation.Code, invitation.ReferenceId);
    }
    
    private async Task RejectEventOrganizationContactAsync(OrganizationInvitation invitation)
    {
        var organization = await _eventOrganizationRepository.FindAsync(new EventOrganizationByContactSpecification(invitation.ReferenceId));
        if (organization == null)
        {
            throw new EventOrganizationNotFoundException(invitation.ReferenceId);
        }

        organization.RejectContactInvitation(invitation.ReferenceId);
        _eventOrganizationRepository.Update(organization);
        _logger.LogInformation("Rejected invitation {InvitationCode} for organization contact {OrganizationContactId}!", invitation.Code, invitation.ReferenceId);
    }
}