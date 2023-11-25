using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ServeSync.Application.QueryObjects;
using ServeSync.Application.ReadModels.Events;
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
    
    public async Task<EventReadModel?> GetEventReadModelByIdAsync(Guid eventId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        EventReadModel? @event = null;
        
        await connection.QueryAsync
        <dynamic,
            BasicEventOrganizationInEventReadModel, 
            EventOrganizationInEventReadModel, 
            EventOrganizationRepresentativeInEventReadModel, 
            EventAttendanceInfoReadModel, 
            EventRegistrationInfoReadModel, 
            EventReadModel>(
            GetQueryString(), 
            (row, 
                eventRepresentative, 
                organizationInEvent, 
                representativeInOrganization, 
                eventAttendance, 
                eventRegistration) =>
            {
                @event = @event ?? new EventReadModel()
                {
                    Id = row.Id,
                    Name = row.Name,
                    Introduction = row.Introduction,
                    Description = row.Description,
                    ImageUrl = row.ImageUrl,
                    StartAt = row.StartAt,
                    EndAt = row.EndAt,
                    Type = (EventType)row.Type,
                    Status = (EventStatus)row.Status,
                    Address = new EventAddressReadModel()
                    {
                        FullAddress = row.Address_FullAddress,
                        Latitude = row.Address_Latitude,
                        Longitude = row.Address_longitude
                    },
                    RepresentativeOrganization = eventRepresentative,
                    Activity = new EventActivityReadModel()
                    {
                        Id = row.ActivityId,
                        Name = row.ActivityName,
                        MinScore = row.ActivityMinScore,
                        MaxScore = row.ActivityMaxScore,
                        EventCategoryId = row.ActivityCategoryId,
                        EventCategoryName = row.ActivityCategoryName
                    },
                    Created = new AuditReadModel()
                    {
                        Id = row.CreatedBy,
                        At = row.Created,
                        FullName = row.CreatedUserFullName,
                        ImageUrl = row.CreatedUserAvatarUrl
                    },
                    Modified = row.LastModified != null ? new AuditReadModel()
                    {
                        Id = row.LastModifiedBy,
                        At = row.LastModified,
                        FullName = row.ModifiedUserFullName,
                        ImageUrl = row.ModifiedUserAvatarUrl
                    } : null,
                    Roles = new List<EventRoleReadModel>(),
                    Organizations = new List<EventOrganizationInEventReadModel>(),
                    AttendanceInfos = new List<EventAttendanceInfoReadModel>(),
                    RegistrationInfos = new List<EventRegistrationInfoReadModel>()
                };

                if (@event.Roles.All(x => row.EventRoleId != x.Id))
                {
                    @event.Roles.Add(new EventRoleReadModel()
                    {
                        Id = row.EventRoleId,
                        Name = row.EventRoleName,
                        Description = row.EventRoleDescription,
                        IsNeedApprove = row.EventRoleIsNeedApprove,
                        Quantity = row.EventRoleQuantity,
                        Score = row.EventRoleScore,
                        RegisteredStudents = new List<RegisteredStudentInEventRoleReadModel>()
                    });    
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
                
                var tempEventOrganizationInEvent = @event.Organizations.First(x => x.Id == organizationInEvent.Id);
                tempEventOrganizationInEvent.Representatives = tempEventOrganizationInEvent.Representatives ?? new List<EventOrganizationRepresentativeInEventReadModel>();

                if (tempEventOrganizationInEvent.Representatives.All(x => representativeInOrganization.Id != x.Id))
                {
                    tempEventOrganizationInEvent.Representatives.Add(representativeInOrganization);
                }
                
                if (row.StudentEventRegisterStudentId != null)
                {
                    var tempEventRole = @event.Roles.First(x => x.Id == row.EventRoleId);
                    if (tempEventRole.RegisteredStudents.All(x => row.StudentEventRegisterId != x.Id))
                    {
                        tempEventRole.RegisteredStudents.Add(new RegisteredStudentInEventRoleReadModel()
                        {
                            Id  = row.StudentEventRegisterId,
                            Code = row.StudentEventRegisterCode,
                            StudentId = row.StudentEventRegisterStudentId,
                            Description = row.StudentEventRegisterDescription,
                            RejectReason = row.StudentEventRegisterRejectReason,
                            Status = (EventRegisterStatus)row.StudentEventRegisterStatus,
                            Name = row.StudentEventRegisterFullName,
                            ImageUrl = row.StudentEventRegisterImageUrl,
                            RegisteredAt = row.StudentEventRegisterCreated,
                            IdentityId = row.StudentEventRegisterIdentityId,
                            Email = row.StudentEventRegisterEmail,
                            Phone = row.StudentEventRegisterPhone,
                            HomeRoomName = row.StudentEventRegisterHomeRoomName
                        });    
                    }
                }

                if (row.StudentEventAttendanceInEventAttendanceInfoId != null)
                {
                    var tempEventAttendanceInfo = @event.AttendanceInfos.First(x => x.Id == row.StudentEventAttendanceInEventAttendanceInfoId);
                    tempEventAttendanceInfo.AttendanceStudents = tempEventAttendanceInfo.AttendanceStudents ?? new List<AttendanceStudentInEventRoleReadModel>();
                    if (tempEventAttendanceInfo.AttendanceStudents.All(x => x.Id != row.StudentEventAttendanceId))
                    {
                        tempEventAttendanceInfo.AttendanceStudents.Add(new AttendanceStudentInEventRoleReadModel()
                        {
                            Id = row.StudentEventAttendanceId,
                            Code = row.StudentEventRegisterCode,
                            StudentId = row.StudentEventRegisterStudentId, 
                            Name = row.StudentEventRegisterFullName,
                            ImageUrl = row.StudentEventRegisterImageUrl,
                            AttendanceAt = row.StudentEventAttendanceAttendanceAt,
                            IdentityId = row.StudentEventRegisterIdentityId,
                            Email = row.StudentEventRegisterEmail,
                            Phone = row.StudentEventRegisterPhone,
                            Role = row.EventRoleName,
                            HomeRoomName = row.StudentEventRegisterHomeRoomName,
                            Score = row.EventRoleScore
                        });    
                    }
                }

                return @event;
            },
            splitOn: "EventIdInRepresentativeOrganization, EventIdInOrganizationInEvent, OrganizationInEventIdInOrganizationRepInEvent, EventIdInEventAttendanceInfo, EventIdInEventRegistrationInfo",
            param: new { EventId = eventId });
        
        return @event;
    }

    private string GetQueryString()
    {
        // Todo: move to stored procedure
        return @$"
            SELECT 
	            Event.Id, Event.ActivityId, Event.Description, Event.StartAt, Event.EndAt, Event.ImageUrl, Event.Introduction, Event.Name, Event.RepresentativeOrganizationId, Event.Status, Event.Type, Event.Address_FullAddress, Event.Address_Latitude, Event.Address_longitude, Event.Created, Event.CreatedBy, CreatedUser.FullName as CreatedUserFullName, CreatedUser.AvatarUrl as CreatedUserAvatarUrl, Event.LastModified, Event.LastModifiedBy, ModifiedUser.FullName as ModifiedUserFullName, ModifiedUser.AvatarUrl as ModifiedUserAvatarUrl, Event.ActivityId, EventActivity.Name As ActivityName, EventActivity.MinScore As ActivityMinScore, EventActivity.MaxScore As ActivityMaxScore, EventActivity.EventCategoryId As ActivityCategoryId, EventCategory.Name As ActivityCategoryName,
	            EventRole.EventId As EventIdInRole, EventRole.Id As EventRoleId, EventRole.Name as EventRoleName, EventRole.Quantity as EventRoleQuantity, EventRole.Description as EventRoleDescription, EventRole.IsNeedApprove as EventRoleIsNeedApprove, EventRole.Score as EventRoleScore, 
	            StudentEventRegister.EventRoleId As EventRoleIdInStudentEventRegister, StudentEventRegister.Id As StudentEventRegisterId, StudentEventRegister.StudentId as StudentEventRegisterStudentId, StudentEventRegister.Status as StudentEventRegisterStatus, StudentEventRegister.Description as StudentEventRegisterDescription, StudentEventRegister.RejectReason as StudentEventRegisterRejectReason, StudentEventRegister.Created as StudentEventRegisterCreated, Student.FullName as StudentEventRegisterFullName, Student.ImageUrl as StudentEventRegisterImageUrl, Student.IdentityId as StudentEventRegisterIdentityId, Student.Email as StudentEventRegisterEmail, Student.Phone as StudentEventRegisterPhone, Student.Code as StudentEventRegisterCode, HomeRoom.Name as StudentEventRegisterHomeRoomName,
	            StudentEventAttendance.EventAttendanceInfoId as StudentEventAttendanceInEventAttendanceInfoId, StudentEventAttendance.Id As StudentEventAttendanceId, StudentEventAttendance.AttendanceAt as StudentEventAttendanceAttendanceAt,
	            RepresentativeOrganizationInEvent.EventId As EventIdInRepresentativeOrganization, RepresentativeOrganizationInEvent.Id, RepresentativeOrganizationInEvent.OrganizationId, RepresentativeOrganization.Name, RepresentativeOrganization.ImageUrl, RepresentativeOrganization.Email, RepresentativeOrganization.PhoneNumber, RepresentativeOrganization.Address,
	            OrganizationInEvent.EventId As EventIdInOrganizationInEvent, OrganizationInEvent.Id, OrganizationInEvent.OrganizationId, EventOrganization.Name, OrganizationInEvent.Role, EventOrganization.Name, EventOrganization.ImageUrl, EventOrganization.Email, EventOrganization.PhoneNumber, EventOrganization.Address,
	            OrganizationRepInEvent.OrganizationInEventId As OrganizationInEventIdInOrganizationRepInEvent, OrganizationRepInEvent.Id, OrganizationRepInEvent.OrganizationRepId, EventOrganizationContact.Name, EventOrganizationContact.ImageUrl, OrganizationRepInEvent.Role, EventOrganizationContact.Position, EventOrganizationContact.Email, EventOrganizationContact.PhoneNumber, EventOrganization.Address,
	            EventAttendanceInfo.EventId As EventIdInEventAttendanceInfo, EventAttendanceInfo.Id, EventAttendanceInfo.StartAt, EventAttendanceInfo.EndAt, EventAttendanceInfo.Code, EventAttendanceInfo.QrCodeUrl,
	            EventRegistrationInfo.EventId As EventIdInEventRegistrationInfo, EventRegistrationInfo.Id, EventRegistrationInfo.StartAt, EventRegistrationInfo.EndAt
            From Event
            LEFT JOIN EventActivity
            ON EventActivity.Id = Event.ActivityId
            LEFT JOIN EventCategory
            ON EventCategory.Id = EventActivity.EventCategoryId
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
            LEFT JOIN StudentEventRegister
            ON EventRole.Id = StudentEventRegister.EventRoleId
            LEFT JOIN Student
            ON StudentEventRegister.StudentId = Student.Id
            LEFT JOIN HomeRoom
            ON Student.HomeRoomId = HomeRoom.Id
            LEFT JOIN StudentEventAttendance
            ON StudentEventAttendance.StudentEventRegisterId = StudentEventRegister.Id
            LEFT JOIN AspNetUsers As CreatedUser
            ON Event.CreatedBy = CreatedUser.Id
            LEFT JOIN AspNetUsers As ModifiedUser
            ON Event.LastModifiedBy = ModifiedUser.Id
            WHERE Event.Id = @EventId
        ";
    }
}