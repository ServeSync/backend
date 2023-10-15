using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ServeSync.Application.QueryObjects;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Infrastructure.Dappers.QueryObjects;

public class EventQueries : IEventQueries
{
    private readonly string _connectionString;
    
    public EventQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? throw new Exception("Default connection string is not provided!");
    }
    
    public async Task<EventDetailDto?> GetEventDetailByIdAsync(Guid eventId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        EventDetailDto? @event = null;
        
        await connection.QueryAsync
        <dynamic, EventRoleDto, BasicOrganizationInEventDto, OrganizationInEventDto, BasicRepresentativeInEventDto, EventAttendanceInfoDto, EventRegistrationDto, EventDetailDto>(
            GetQueryString(), (
                row, eventRole, eventRepresentative, organizationInEvent, 
                representativeInOrganization, eventAttendance, eventRegistration) =>
            {
                @event = @event ?? new EventDetailDto()
                {
                    Id = row.Id,
                    Name = row.Name,
                    Introduction = row.Introduction,
                    ImageUrl = row.ImageUrl,
                    StartAt = row.StartAt,
                    EndAt = row.EndAt,
                    Type = (EventType)row.Type,
                    Status = (EventStatus)row.Status,
                    // Capacity = (int)row.Capacity,
                    // Registered = (int)row.Registered,
                    Address = new EventAddressDto()
                    {
                        FullAddress = row.Address_FullAddress,
                        Latitude = row.Address_Latitude,
                        Longitude = row.Address_longitude
                    },
                    RepresentativeOrganization = eventRepresentative,
                    Activity = new BasicEventActivityDto()
                    {
                        Id = row.ActivityId,
                        Name = row.ActivityName  
                    },
                    Roles = new List<EventRoleDto>(),
                    Organizations = new List<OrganizationInEventDto>(),
                    AttendanceInfos = new List<EventAttendanceInfoDto>(),
                    RegistrationInfos = new List<EventRegistrationDto>()
                };

                if (@event.Roles.All(x => eventRole.Id != x.Id))
                {
                    @event.Roles.Add(eventRole);    
                }
                
                if (@event.Organizations.All(x => organizationInEvent.Id != x.Id))
                {
                    @event.Organizations.Add(organizationInEvent);
                }
                
                if (@event.AttendanceInfos.All(x => eventAttendance.Id != x.Id))
                {
                    @event.AttendanceInfos.Add(eventAttendance);
                }

                if (@event.RegistrationInfos.All(x => eventRegistration.Id != x.Id))
                {
                    @event.RegistrationInfos.Add(eventRegistration);
                }
                
                var temp = @event.Organizations.First(x => x.Id == organizationInEvent.Id);
                temp.Representatives = temp.Representatives ?? new List<BasicRepresentativeInEventDto>();

                if (temp.Representatives.All(x => representativeInOrganization.Id != x.Id))
                {
                    temp.Representatives.Add(representativeInOrganization);
                }

                return @event;
            },
            splitOn: "EventIdInRole, EventIdInRepresentativeOrganization, EventIdInOrganizationInEvent, OrganizationInEventIdInOrganizationRepInEvent, EventIdInEventAttendanceInfo, EventIdInEventRegistrationInfo",
            param: new { EventId = eventId, RegisterStatus = EventRegisterStatus.Approved });

        if (@event != null)
        {
            @event!.Registered = @event.Roles.Sum(x => x.Registered);
            @event.Capacity = @event.Roles.Sum(x => x.Quantity);    
            @event.Status = @event.GetCurrentStatus(DateTime.Now);
        }
        
        return @event;
    }

    private string GetQueryString()
    {
        // -- (SELECT SUM(EventRole.Quantity) From EventRole Where EventId = Event.Id) As Capacity,
        // -- (SELECT SUM(MyCount) FROM (SELECT COUNT(*) As MyCount FROM StudentEventRegister Where Status = 1 and EventRoleId IN (SELECT Id From EventRole Where EventId = Event.Id)) as temp) as Registered,
        // Todo: move to stored procedure
        return @$"
            SELECT 
	            Event.Id, Event.ActivityId, Event.Description, Event.StartAt, Event.EndAt, Event.ImageUrl, Event.Introduction, Event.Name, Event.RepresentativeOrganizationId, Event.Status, Event.Type, Event.Address_FullAddress, Event.Address_Latitude, Event.Address_longitude, Event.ActivityId, EventActivity.Name As ActivityName,
	            EventRole.EventId As EventIdInRole, EventRole.Id, EventRole.Name, EventRole.Quantity, EventRole.Description, EventRole.IsNeedApprove, EventRole.Score, (SELECT COUNT(StudentEventRegister.Id) FROM StudentEventRegister Where EventRoleId = EventRole.Id and Status = @RegisterStatus) AS Registered,
                RepresentativeOrganizationInEvent.EventId As EventIdInRepresentativeOrganization, RepresentativeOrganizationInEvent.Id, RepresentativeOrganizationInEvent.OrganizationId, RepresentativeOrganization.Name, RepresentativeOrganization.ImageUrl,
                OrganizationInEvent.EventId As EventIdInOrganizationInEvent,OrganizationInEvent.Id, OrganizationInEvent.OrganizationId, EventOrganization.Name, OrganizationInEvent.Role, EventOrganization.Name, EventOrganization.ImageUrl,
                OrganizationRepInEvent.OrganizationInEventId As OrganizationInEventIdInOrganizationRepInEvent, OrganizationRepInEvent.Id, OrganizationRepInEvent.OrganizationRepId, EventOrganizationContact.Name, EventOrganizationContact.ImageUrl, OrganizationRepInEvent.Role, EventOrganizationContact.Position,
                EventAttendanceInfo.EventId As EventIdInEventAttendanceInfo, EventAttendanceInfo.Id, EventAttendanceInfo.StartAt, EventAttendanceInfo.EndAt, EventAttendanceInfo.Code, EventAttendanceInfo.QrCodeUrl,
                EventRegistrationInfo.EventId As EventIdInEventRegistrationInfo, EventRegistrationInfo.Id, EventRegistrationInfo.StartAt, EventRegistrationInfo.EndAt
            From Event
            LEFT JOIN EventActivity
            ON EventActivity.Id = Event.ActivityId
            LEFT JOIN OrganizationInEvent as RepresentativeOrganizationInEvent
            ON Event.RepresentativeOrganizationId = RepresentativeOrganizationInEvent.Id
            LEFT JOIN EventOrganization as RepresentativeOrganization
            On RepresentativeOrganizationInEvent.OrganizationId = RepresentativeOrganization.Id
            LEFT JOIN OrganizationInEvent
            ON OrganizationInEvent.EventId = Event.Id
            LEFT JOIN EventOrganization
            ON OrganizationInEvent.OrganizationId = EventOrganization.Id
            LEFT JOIN OrganizationRepInEvent
            ON OrganizationRepInEvent.OrganizationInEventId = OrganizationInEvent.Id
            LEFT JOIN EventOrganizationContact
            ON OrganizationRepInEvent.OrganizationRepId = EventOrganizationContact.Id
            LEFT JOIN EventAttendanceInfo
            ON EventAttendanceInfo.EventId = Event.Id
            LEFT JOIN EventRegistrationInfo
            ON EventRegistrationInfo.EventId = Event.Id
            LEFT JOIN EventRole 
            On EventRole.EventId = Event.Id
            WHERE Event.Id = @EventId
        ";
    }
}