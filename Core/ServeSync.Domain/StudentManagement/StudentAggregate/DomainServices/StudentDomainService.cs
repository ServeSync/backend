using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

public class StudentDomainService : IStudentDomainService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IHomeRoomRepository _homeRoomRepository;
    private readonly IEducationProgramRepository _educationProgramRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IBasicReadOnlyRepository<EventRole, Guid> _eventRoleRepository;
    private readonly IBasicReadOnlyRepository<StudentEventRegister, Guid> _studentEventRegisterRepository;

    public StudentDomainService(
        IStudentRepository studentRepository,
        IHomeRoomRepository homeRoomRepository,
        IEducationProgramRepository educationProgramRepository,
        IEventRepository eventRepository,
        IBasicReadOnlyRepository<EventRole, Guid> eventRoleRepository,
        IBasicReadOnlyRepository<StudentEventRegister, Guid> studentEventRegisterRepository)
    {
        _studentRepository = studentRepository;
        _homeRoomRepository = homeRoomRepository;
        _educationProgramRepository = educationProgramRepository;
        _eventRepository = eventRepository;
        _eventRoleRepository = eventRoleRepository;
        _studentEventRegisterRepository = studentEventRegisterRepository;
    }
    
    public async Task<Student> CreateAsync(
        string code, 
        string fullName, 
        bool gender, 
        DateTime dateOfBirth, 
        string imageUrl, 
        string citizenId,
        string email, 
        string phone, 
        Guid homeRoomId, 
        Guid educationProgramId,
        string? homeTown = null,
        string? address = null)
    {
        await CheckHomeRoomExistsAsync(homeRoomId);
        await CheckEducationProgramExistsAsync(educationProgramId);
        
        await CheckDuplicateCodeAsync(code);
        await CheckDuplicateEmailAsync(email);
        await CheckDuplicateCitizenIdentifierAsync(citizenId);

        var student = new Student(
            code, 
            fullName,
            gender, 
            dateOfBirth, 
            imageUrl, 
            citizenId,
            email, 
            phone, 
            homeRoomId, 
            educationProgramId,
            homeTown,
            address);
        
        await _studentRepository.InsertAsync(student);
        return student;
    }

    public async Task<Student> UpdateContactInfoAsync(
        Student student, 
        string fullName, 
        bool gender, 
        DateTime dateOfBirth, 
        string imageUrl,
        string citizenId,
        string email, 
        string phone, 
        string? homeTown = null, 
        string? address = null)
    {
        if (student.Email != email)
        {
            await CheckDuplicateEmailAsync(email);
        }
        
        if (student.CitizenId != citizenId)
        {
            await CheckDuplicateCitizenIdentifierAsync(citizenId);
        }
        
        student.UpdateContactInfo(fullName, gender, dateOfBirth, imageUrl, citizenId, email, phone, homeTown, address);
        _studentRepository.Update(student);
        return student;
    }

    public async Task<Student> UpdateEducationInfoAsync(Student student, string code, Guid homeRoomId, Guid educationProgramId)
    {
        if (student.Code != code)
        {
            await CheckDuplicateCodeAsync(code);
        }
        
        if (student.HomeRoomId != homeRoomId)
        {
            await CheckHomeRoomExistsAsync(homeRoomId);
        }
        
        if (student.EducationProgramId != educationProgramId)
        {
            await CheckEducationProgramExistsAsync(educationProgramId);
        }
        
        student.UpdateEducationInfo(code, educationProgramId, homeRoomId);
        _studentRepository.Update(student);
        return student;
    }

    public void Delete(Student student)
    {
        _studentRepository.Delete(student);
        student.AddDomainEvent(new StudentDeletedDomainEvent(student.Id, student.IdentityId));
    }

    public async Task SetIdentityAsync(Student student, string identityId)
    {
        await CheckDuplicateIdentityAsync(identityId);
        
        student.WithIdentity(identityId);
        _studentRepository.Update(student);
    }

    public async Task<Student> RegisterEventAsync(Student student, Guid eventRoleId, string? description, DateTime currentDateTime)
    {
        var eventRole = await _eventRoleRepository.FindAsync(new EventRoleByIdSpecification(eventRoleId));
        if (eventRole == null)
        {
            throw new EventRoleNotFoundException(eventRoleId);
        }

        if (!eventRole.Event!.CanRegister(currentDateTime))
        {
            throw new NotInEventRegistrationTimeException(eventRole.EventId);
        }

        if (eventRole.Event.Roles.Any(x => student.IsApprovedToEventRole(x.Id)))
        {
            throw new StudentHasAlreadyApprovedToEventException(student.Id, eventRole.EventId);
        }

        var approvedStudentCount = await _studentEventRegisterRepository.GetCountAsync(new ApprovedStudentInEventRoleSpecification(eventRoleId));
        if (approvedStudentCount >= eventRole.Quantity)
        {
            throw new EventRoleIsFullApprovedException(eventRoleId);
        }
        
        if (!eventRole.IsNeedApprove)
        {
            student.RegisterEventWithApprove(eventRoleId, description);    
        }
        else
        {
            student.RegisterEvent(eventRoleId, description);
        }
        
        _studentRepository.Update(student);
        return student;
    }

    public async Task<Student> AttendEventAsync(
        Student student, 
        Guid eventId, 
        string code, 
        DateTime currentDateTime,
        double longitude,
        double latitude)
    {
        var @event = await _eventRepository.FindByIdAsync(eventId);
        if (@event == null)
        {
            throw new EventNotFoundException(eventId);
        }

        if (!@event.Roles.Any(x => student.IsApprovedToEventRole(x.Id)))
        {
            throw new StudentNotApprovedToEventException(student.Id, eventId);
        }
        
        if (!@event.IsInAttendArea(longitude, latitude))
        {
            throw new StudentNotInAttendEventAreaException(student.Id, eventId);
        }
        
        if (!@event.ValidateCode(code, currentDateTime))
        {
            throw new InvalidEventAttendanceCodeException(code);
        }
        
        var eventAttendanceId = @event.AttendanceInfos.First(x => x.ValidateCode(code, currentDateTime)).Id;
        var eventRoleId = @event.Roles.First(x => student.IsApprovedToEventRole(x.Id)).Id;
        
        student.AttendEvent(eventRoleId, eventAttendanceId);

        return student;
    }

    public async Task<Student> ApproveEventRegisterAsync(Student student, Guid eventRegisterId, DateTime currentDateTime)
    {
        var eventRegister = student.EventRegisters.FirstOrDefault(x => x.Id == eventRegisterId);
        if (eventRegister == null)
        {
            throw new StudentEventRegisterNotFoundException(eventRegisterId);
        }

        var @event = await _eventRepository.FindAsync(new EventByRoleSpecification(eventRegister.EventRoleId));
        if (@event!.StartAt <= currentDateTime || @event.Status != EventStatus.Approved)
        {
            throw new EventHasAlreadyStartedException(@event.Id);
        }
        
        var eventRole = @event.Roles.First(x => x.Id == eventRegister.EventRoleId);
        var approvedStudentCount = await _studentEventRegisterRepository.GetCountAsync(new ApprovedStudentInEventRoleSpecification(eventRegister.EventRoleId));
        if (approvedStudentCount >= eventRole.Quantity)
        {
            throw new EventRoleIsFullApprovedException(eventRegister.EventRoleId);
        }
        
        if (student.IsApprovedToEventRole(eventRegister.EventRoleId))
        {
            throw new StudentHasAlreadyApprovedToEventException(student.Id, @event.Id);
        }
            
        student.ApproveEventRegister(eventRegisterId);
        
        foreach (var register in student.EventRegisters.Where(x => x.Id != eventRegisterId && @event.Roles.Any(y => y.Id == x.EventRoleId)))
        {
            register.Reject("Đã tham gia với vai trò khác!");
        }
        return student;
    }

    public async Task<Student> RejectEventRegisterAsync(Student student, Guid eventRegisterId, string reason, DateTime currentDateTime)
    {
        var eventRegister = student.EventRegisters.FirstOrDefault(x => x.Id == eventRegisterId);
        if (eventRegister == null)
        {
            throw new StudentEventRegisterNotFoundException(eventRegisterId);
        }

        var @event = await _eventRepository.FindAsync(new EventByRoleSpecification(eventRegister.EventRoleId));
        if (@event!.StartAt <= currentDateTime || @event.Status != EventStatus.Approved)
        {
            throw new EventHasAlreadyStartedException(@event.Id);
        }
        
        student.RejectEventRegister(eventRegisterId, reason);
        return student;
    }

    private async Task CheckDuplicateCodeAsync(string code)
    {
        if (await _studentRepository.AnyAsync(new StudentByCodeSpecification(code)))
        {
            throw new DuplicateStudentCodeException(code);
        }
    }

    private async Task CheckDuplicateCitizenIdentifierAsync(string citizenId)
    {
        if (await _studentRepository.AnyAsync(new StudentByCitizenIdentifierSpecification(citizenId)))
        {
            throw new DuplicateStudentCitizenIdentifierException(citizenId);
        }
    }

    private async Task CheckDuplicateEmailAsync(string email)
    {
        if (await _studentRepository.AnyAsync(new StudentByEmailSpecification(email)))
        {
            throw new DuplicateStudentEmailException(email);
        }
    }

    private async Task CheckDuplicateIdentityAsync(string identityId)
    {
        if (await _studentRepository.AnyAsync(new StudentByIdentitySpecification(identityId)))
        {
            throw new DuplicateStudentIdentityException(identityId);
        }
    }
    
    private async Task CheckHomeRoomExistsAsync(Guid homeRoomId)
    {
        if (!await _homeRoomRepository.IsExistingAsync(homeRoomId))
        {
            throw new HomeRoomNotFoundException(homeRoomId);
        }
    }
    
    private async Task CheckEducationProgramExistsAsync(Guid educationProgramId)
    {
        if (!await _educationProgramRepository.IsExistingAsync(educationProgramId))
        {
            throw new EducationProgramNotFoundException(educationProgramId);
        }
    }
}