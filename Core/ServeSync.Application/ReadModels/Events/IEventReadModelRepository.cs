using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public interface IEventReadModelRepository : IReadModelRepository<EventReadModel, Guid>
{
    Task<EventRoleReadModel?> GetEventRoleByIdAsync(Guid eventRoleId);
    
    Task<List<EventRoleReadModel>?> GetEventRolesAsync(Guid eventId);
    
    Task<(List<RegisteredStudentInEventReadModel>?, int?)> GetPagedRegisteredStudentsInEventAsync(Guid eventId, int page, int size); 
    
    Task<(List<AttendanceStudentInEventRoleReadModel>?, int?)> GetPagedAttendanceStudentsInEventAsync(Guid eventId, int page, int size);

    Task<(List<EventReadModel>, int)> GetAttendanceEventsOfStudentAsync(Guid studentId, int page, int size);
    
    Task<int> GetCountNumberOfAttendedEventsOfStudentAsync(Guid studentId);
    
    Task<double> GetSumScoreOfAttendedEventsOfStudentAsync(Guid requestStudentId);

    Task<EventReadModel?> GetEventByStudentRegistrationAsync(Guid eventRegisterId);
}