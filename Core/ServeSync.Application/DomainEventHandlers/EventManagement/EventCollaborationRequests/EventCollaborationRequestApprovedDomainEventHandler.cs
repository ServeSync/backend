using Microsoft.Extensions.Logging;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Events;

namespace ServeSync.Application.DomainEventHandlers.EventManagement.EventCollaborationRequests;

public class EventCollaborationRequestApprovedDomainEventHandler : IDomainEventHandler<EventCollaborationRequestApprovedDomainEvent>
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IEventDomainService _eventDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ILogger<EventCollaborationRequestApprovedDomainEventHandler> _logger;
    
    public EventCollaborationRequestApprovedDomainEventHandler(
        IEventOrganizationRepository eventOrganizationRepository,
        IEventRepository eventRepository,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IEventDomainService eventDomainService,
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator,
        ILogger<EventCollaborationRequestApprovedDomainEventHandler> logger)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventRepository = eventRepository;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _eventDomainService = eventDomainService;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _logger = logger;
    }
    
    public async Task Handle(EventCollaborationRequestApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var organization = await CreateOrganizationAsync(notification);
        var @event = await CreateEventAsync(notification, organization);
        
        notification.EventCollaborationRequest.SetEventId(@event.Id);
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.EventCollaborationRequest.OrganizationContact.Email,
            Subject = "[ServeSync] Thông báo duyệt đơn hợp tác",
            Body = _emailTemplateGenerator.GetApproveCollaborationRequest(notification.EventCollaborationRequest.OrganizationContact.Name, notification.EventCollaborationRequest.Name)
        });
        
        _emailSender.Push(new EmailMessage()
        {
            ToAddress = notification.EventCollaborationRequest.Organization.Email,
            Subject = "[ServeSync] Thông báo duyệt đơn hợp tác",
            Body = _emailTemplateGenerator.GetApproveCollaborationRequest(notification.EventCollaborationRequest.OrganizationContact.Name, notification.EventCollaborationRequest.Name)
        });
    }
    
    private async Task<EventOrganization> CreateOrganizationAsync(EventCollaborationRequestApprovedDomainEvent notification)
    {
        var organization = await _eventOrganizationRepository.FindByEmailAsync(notification.EventCollaborationRequest.Organization.Email);
        if (organization == null)
        {
            organization = await _eventOrganizationDomainService.CreateAsync(
                notification.EventCollaborationRequest.Organization.Name,
                notification.EventCollaborationRequest.Organization.Email,
                notification.EventCollaborationRequest.Organization.PhoneNumber,
                notification.EventCollaborationRequest.Organization.ImageUrl,
                notification.EventCollaborationRequest.Organization.Description,
                notification.EventCollaborationRequest.Organization.Address);
            
            organization.ApproveInvitation();
            
            _eventOrganizationDomainService.AddContact(
                organization,
                notification.EventCollaborationRequest.OrganizationContact.Name,
                notification.EventCollaborationRequest.OrganizationContact.Email,
                notification.EventCollaborationRequest.OrganizationContact.PhoneNumber,
                notification.EventCollaborationRequest.OrganizationContact.ImageUrl,
                notification.EventCollaborationRequest.OrganizationContact.Gender,
                notification.EventCollaborationRequest.OrganizationContact.Birth,
                notification.EventCollaborationRequest.OrganizationContact.Address,
                notification.EventCollaborationRequest.OrganizationContact.Position).ApproveInvitation();
            
            await _eventOrganizationRepository.InsertAsync(organization);
            _logger.LogInformation("Created new organization {Email}", notification.EventCollaborationRequest.Organization.Email);
        }
        else
        {
            try
            {
                _eventOrganizationDomainService.AddContact(
                    organization,
                    notification.EventCollaborationRequest.OrganizationContact.Name,
                    notification.EventCollaborationRequest.OrganizationContact.Email,
                    notification.EventCollaborationRequest.OrganizationContact.PhoneNumber,
                    notification.EventCollaborationRequest.OrganizationContact.ImageUrl,
                    notification.EventCollaborationRequest.OrganizationContact.Gender,
                    notification.EventCollaborationRequest.OrganizationContact.Birth,
                    notification.EventCollaborationRequest.OrganizationContact.Address,
                    notification.EventCollaborationRequest.OrganizationContact.Position).ApproveInvitation();
                _eventOrganizationRepository.Update(organization);
            }
            catch (EventOrganizationContactAlreadyExistedException e)
            {
                _logger.LogInformation(e.Message);
            }    
        }

        return organization;
    }
    
    private async Task<Event> CreateEventAsync(
        EventCollaborationRequestApprovedDomainEvent notification,
        EventOrganization organization)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var dateTime = DateTime.UtcNow;
            
            var @event = await _eventDomainService.CreateAsync(
                notification.EventCollaborationRequest.Name,
                notification.EventCollaborationRequest.Introduction,
                notification.EventCollaborationRequest.Description,
                notification.EventCollaborationRequest.ImageUrl,
                notification.EventCollaborationRequest.Type,
                notification.EventCollaborationRequest.StartAt,
                notification.EventCollaborationRequest.EndAt,
                notification.EventCollaborationRequest.ActivityId,
                notification.EventCollaborationRequest.Address.FullAddress,
                notification.EventCollaborationRequest.Address.Longitude,
                notification.EventCollaborationRequest.Address.Latitude);
            
            _eventDomainService.AddRegistrationInfo(
                @event,
                notification.EventCollaborationRequest.StartAt.AddDays(-1), 
                notification.EventCollaborationRequest.StartAt.AddDays(-1).AddMinutes(15)
                , DateTime.UtcNow);
            
            _eventDomainService.AddAttendanceInfo(
                @event, 
                notification.EventCollaborationRequest.StartAt, 
                notification.EventCollaborationRequest.StartAt.AddMinutes(15),
                dateTime);

            await _eventDomainService.AddDefaultRoleAsync(@event, notification.EventCollaborationRequest.Capacity, dateTime);
            
            _eventDomainService.AddOrganization(@event, organization, "Nhà tổ chức", dateTime);
            
            _eventDomainService.AddRepresentative(
                @event, 
                organization,
                organization.Contacts.First(x => x.Email == notification.EventCollaborationRequest.OrganizationContact.Email),
                "Diễn giả",
                dateTime);
            
            await _eventRepository.InsertAsync(@event);
            await _unitOfWork.CommitAsync();

            _eventDomainService.SetRepresentativeOrganization(@event, organization.Id, dateTime);
            @event.Create(organization.IdentityId!);
            _eventDomainService.ApproveEvent(@event, DateTime.UtcNow);
            
            _eventRepository.Update(@event);
            
            await _unitOfWork.CommitTransactionAsync();
            
            _logger.LogInformation("Convert collaboration request '{Id}' to event success with id: {EventId}", notification.EventCollaborationRequest.Id, @event.Id);
            return @event;
        }
        catch (Exception e)
        {
            _logger.LogInformation("Convert collaboration request to event failed: {Message}", e.Message);
            throw;
        }
    }
}