using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;

public class GetEducationProgramByStudentQuery : IQuery<StudentEducationProgramDto>
{
    public Guid StudentId { get; set; }
    
    public GetEducationProgramByStudentQuery(Guid studentId)
    {
        StudentId = studentId;
    }
}