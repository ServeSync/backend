using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

public interface IStudentDomainService
{
    Task<Student> CreateAsync(
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
        string? address = null);

    Task<Student> UpdateContactInfoAsync(
        Student student,
        string fullName,
        bool gender,
        DateTime dateOfBirth,
        string imageUrl,
        string citizenId,
        string email,
        string phone,
        string? homeTown = null,
        string? address = null);
    
    Task<Student> UpdateEducationInfoAsync(
        Student student,
        string code,
        Guid homeRoomId,
        Guid educationProgramId);

    void Delete(Student student);

    Task SetIdentityAsync(Student student, string identityId);
}