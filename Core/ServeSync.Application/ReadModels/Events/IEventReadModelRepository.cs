using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.ReadModels.Events;

public interface IEventReadModelRepository : IReadModelRepository<EventReadModel, Guid>
{
    Task<EventRoleReadModel?> GetEventRoleByIdAsync(Guid eventRoleId);
    
    Task<List<EventRoleReadModel>?> GetEventRolesAsync(Guid eventId);
    
    Task<(List<RegisteredStudentInEventReadModel>?, int?)> GetPagedRegisteredStudentsInEventAsync(Guid eventId, EventRegisterStatus? status, int page, int size); 
    
    Task<(List<AttendanceStudentInEventRoleReadModel>?, int?)> GetPagedAttendanceStudentsInEventAsync(Guid eventId, int page, int size);

    Task<(List<EventReadModel>, int)> GetAttendanceEventsOfStudentAsync(Guid studentId, int page, int size, bool isPaging);
    
    Task<List<EventReadModel>> GetAttendanceEventsOfStudentAsync(Guid studentId, DateTime? startAt, DateTime? endAt);
    
    Task<(List<EventReadModel>, int)> GetRegisteredEventsOfStudentAsync(Guid studentId, int page, int size);
    
    Task<int> GetCountNumberOfAttendedEventsOfStudentAsync(Guid studentId);
    
    Task<double> GetSumScoreOfAttendedEventsOfStudentAsync(Guid requestStudentId);

    Task<EventReadModel?> GetEventByStudentRegistrationAsync(Guid eventRegisterId);

    Task UpdateStudentInEventsAsync(Student student);

    Task UpdateOrganizationInEventsAsync(EventOrganization eventOrganization);
    
    Task UpdateOrganizationContactInEventsAsync(EventOrganizationContact eventOrganizationContact);
}