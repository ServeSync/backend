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

public class ApproveEventOrganizationInvitationCommandHandler : ICommandHandler<ApproveEventOrganizationInvitationCommand>
{
    private readonly IOrganizationInvitationRepository _organizationInvitationRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveEventOrganizationInvitationCommandHandler> _logger;
    
    public ApproveEventOrganizationInvitationCommandHandler(
        IOrganizationInvitationRepository organizationInvitationRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IUnitOfWork unitOfWork,
        ILogger<ApproveEventOrganizationInvitationCommandHandler> logger)
    {
        _organizationInvitationRepository = organizationInvitationRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(ApproveEventOrganizationInvitationCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var invitation = await _organizationInvitationRepository.FindByCodeAsync(request.Code);
        if (invitation == null)
        {
            throw new OrganizationInvitationNotFoundException(request.Code);
        }

        _eventOrganizationDomainService.ProcessInvitation(invitation);
        if (invitation.Type == InvitationType.Organization)
        {
            await ApproveEventOrganizationAsync(invitation);
        }
        else
        {
            await ApproveEventOrganizationContactAsync(invitation);
        }
        
        await _unitOfWork.CommitTransactionAsync(true);
        
        _logger.LogInformation("Approved invitation {InvitationCode}!", request.Code);
    }

    private async Task ApproveEventOrganizationAsync(OrganizationInvitation invitation)
    {
        var organization = await _eventOrganizationRepository.FindByIdAsync(invitation.ReferenceId);
        if (organization == null)
        {
            throw new EventOrganizationNotFoundException(invitation.ReferenceId);
        }

        organization.ApproveInvitation();
        _eventOrganizationRepository.Update(organization);
        _logger.LogInformation("Approved invitation {InvitationCode} for organization {OrganizationId}!", invitation.Code, invitation.ReferenceId);
    }

    private async Task ApproveEventOrganizationContactAsync(OrganizationInvitation invitation)
    {
        var organization = await _eventOrganizationRepository.FindAsync(new EventOrganizationByContactSpecification(invitation.ReferenceId));
        if (organization == null)
        {
            throw new EventOrganizationContactNotFoundException(invitation.ReferenceId);
        }

        organization.ApproveContactInvitation(invitation.ReferenceId);
        _eventOrganizationRepository.Update(organization);
        _logger.LogInformation("Approved invitation {InvitationCode} for organization contact {OrganizationContactId}!", invitation.Code, invitation.ReferenceId);
    }
}