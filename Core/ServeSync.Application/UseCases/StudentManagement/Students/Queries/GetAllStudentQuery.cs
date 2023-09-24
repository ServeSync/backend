using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Queries;

public class GetAllStudentQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<StudentDetailDto>>
{
    public Guid? HomeRoomId { get; set; }
    public Guid? FacultyId { get; set; }
    public Guid? EducationProgramId { get; set; }
    public bool? Gender { get; set; }
    public string? Search { get; set; }
    
    public GetAllStudentQuery(
        Guid? homeRoomId, 
        Guid? facultyId, 
        Guid? educationProgramId, 
        bool? gender, 
        string? search, 
        int page, 
        int size, 
        string sorting) 
        : base(page, size, sorting)
    {
        HomeRoomId = homeRoomId;
        FacultyId = facultyId;
        EducationProgramId = educationProgramId;
        Gender = gender;
        Search = search;
    }
}