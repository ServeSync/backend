using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

public class ProofDomainService : IProofDomainService
{
    private readonly IProofRepository _proofRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly IEventRepository _eventRepository;
    
    public ProofDomainService(
        IProofRepository proofRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        IEventRepository eventRepository)
    {
        _proofRepository = proofRepository;
        _studentRepository = studentRepository;
        _eventRoleRepository = eventRoleRepository;
        _eventRepository = eventRepository;
    }
    
    public async Task<Proof> CreateInternalProofAsync(
        string? description, 
        string imageUrl, 
        DateTime attendanceAt, 
        Guid studentId, 
        Guid eventId,
        Guid eventRoleId,
        DateTime dateTime)
    {
        await CheckStudentExistAsync(studentId);
        await CheckRegisteredEventAsync(eventId, studentId, eventRoleId);
        await CheckIsAttendedToEventAsync(eventId, studentId, DateTime.UtcNow);
        await CheckInternalProofExistAsync(eventId, studentId);
        
        var proof = new InternalProof(
            description,
            imageUrl,
            attendanceAt,
            studentId,
            eventId,
            eventRoleId);

        return proof;
    }
    
    private async Task CheckInternalProofExistAsync(Guid eventId, Guid studentId)
    {
        var isInternalProofExist = await _proofRepository.IsInternalProofExistAsync(eventId, studentId);
        if (isInternalProofExist)
        {
            throw new InternalProofAlreadyExistException(eventId, studentId);
        }
    }
    
    private async Task CheckStudentExistAsync(Guid studentId)
    {
        var isStudentExist = await _studentRepository.IsExistingAsync(studentId);
        if (!isStudentExist)
        {
            throw new StudentNotFoundException(studentId);
        }
    }
    
    private async Task CheckRegisteredEventAsync(Guid eventId, Guid studentId, Guid eventRoleId)
    {
        var isStudentRegistered = await _eventRepository.IsStudentRegisteredToEventAsync(studentId, eventId, eventRoleId);
        if (!isStudentRegistered)
        {
            throw new StudentEventRegisterNotFoundException(studentId, eventId, eventRoleId);
        }
    }
    
    private async Task CheckIsAttendedToEventAsync(Guid eventId, Guid studentId, DateTime dateTime)
    {
        var @event = await _eventRepository.FindByIdAsync(eventId);
        if (@event == null)
        {
            throw new EventNotFoundException(eventId);
        }

        if (@event.AttendanceInfos.Any(x => x.EndAt > dateTime))
        {
            throw new EventIsHappeningException(eventId);
        }
        
        var isStudentAttended = await _eventRepository.IsStudentAttendedToEventAsync(studentId, eventId);
        if (isStudentAttended)
        {
            throw new StudentAlreadyAttendanceException(studentId, eventId);
        }
    }
}