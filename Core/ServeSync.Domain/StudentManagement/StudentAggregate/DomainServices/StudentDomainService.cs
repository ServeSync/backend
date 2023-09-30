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

    public StudentDomainService(
        IStudentRepository studentRepository,
        IHomeRoomRepository homeRoomRepository,
        IEducationProgramRepository educationProgramRepository)
    {
        _studentRepository = studentRepository;
        _homeRoomRepository = homeRoomRepository;
        _educationProgramRepository = educationProgramRepository;
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
        string? address = null,
        bool autoSave = true)
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

        if (autoSave)
        {
            await _studentRepository.InsertAsync(student);
        }
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