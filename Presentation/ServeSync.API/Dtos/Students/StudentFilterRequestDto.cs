using ServeSync.API.Common.Validations;
using ServeSync.Application.Common.Dtos;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.API.Dtos.Students;

public class StudentFilterRequestDto : PagingAndSortingRequestDto
{
    public Guid? HomeRoomId { get; set; }
    public Guid? FacultyId { get; set; }
    public Guid? EducationProgramId { get; set; }
    public bool? Gender { get; set; }
    public string? Search { get; set; }

    [SortConstraint(Fields = $"{nameof(Student.Code)}, {nameof(Student.FullName)}, {nameof(Student.Address)}, {nameof(Student.Gender)}, {nameof(Student.DateOfBirth)}, {nameof(Student.HomeTown)}, {nameof(Student.Phone)}, {nameof(Faculty)}, {nameof(HomeRoom)}, {nameof(EducationProgram)}")]
    public override string? Sorting { get; set; } = string.Empty;
}