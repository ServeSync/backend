using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

public class ProofDomainService : IProofDomainService
{
    private readonly IProofRepository _proofRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    private readonly IEventRepository _eventRepository;
    
    public ProofDomainService(
        IProofRepository proofRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        IEventRepository eventRepository)
    {
        _proofRepository = proofRepository;
        _studentRepository = studentRepository;
        _eventRoleRepository = eventRoleRepository;
        _eventActivityRepository = eventActivityRepository;
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
        await CheckIsAttendedToEventAsync(eventId, studentId, dateTime);
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

    public async Task<Proof> CreateExternalProofAsync(
        string? description, 
        string imageUrl, 
        DateTime? attendanceAt, 
        Guid studentId,
        string eventName, 
        string organizationName,
        string address, 
        string role, 
        DateTime startAt, 
        DateTime endAt, 
        double score, 
        Guid activityId)
    {
        await CheckStudentExistAsync(studentId);
        await CheckValidActivityAsync(activityId, score);
        
        var proof = new ExternalProof(
            description,
            imageUrl,
            attendanceAt,
            studentId,
            eventName,
            organizationName,
            address,
            role,
            startAt,
            endAt,
            score,
            activityId);
        
        return proof;
    }

    public async Task<Proof> CreateSpecialProofAsync(
        string? description, 
        string imageUrl, 
        Guid studentId, 
        string title, 
        string role,
        DateTime startAt, 
        DateTime endAt, 
        double score, 
        Guid activityId)
    {
        await CheckStudentExistAsync(studentId);
        await CheckValidActivityAsync(activityId, score);
        
        var proof = new SpecialProof(
            description,
            imageUrl,
            studentId,
            title,
            role,
            startAt,
            endAt,
            score,
            activityId);

        return proof;
    }

    public Proof RejectProof(Proof proof, string reason)
    {
        proof.Reject(reason);

        return proof;
    }

    public Proof ApproveProof(Proof proof)
    {
        proof.Approve();

        return proof;
    }

    public void Delete(Proof proof)
    {
        if (proof.ProofStatus == ProofStatus.Pending)
        {
            _proofRepository.Delete(proof);
        }
        else
        {
            throw new ProofNotPendingException(proof.Id);
        }
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

        if (@event.GetCurrentStatus(dateTime) != EventStatus.Done)
        {
            throw new EventIsNotDoneException(eventId);
        }
        
        var isStudentAttended = await _eventRepository.IsStudentAttendedToEventAsync(studentId, eventId);
        if (isStudentAttended)
        {
            throw new StudentAlreadyAttendanceException(studentId, eventId);
        }
    }

    private async Task CheckValidActivityAsync(Guid activityId, double score)
    {
        var activity = await _eventActivityRepository.FindByIdAsync(activityId);
        if (activity == null)
        {
            throw new EventActivityNotFoundException(activityId);
        }

        if (score < activity.MinScore || score > activity.MaxScore)
        {
            throw new EventActivityScoreOutOfRangeException(activityId, score);
        }
    }
}