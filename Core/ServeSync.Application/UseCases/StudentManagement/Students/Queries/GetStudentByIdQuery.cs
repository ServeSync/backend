using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Queries;

public class GetStudentByIdQuery : IQuery<FlatStudentDto>
{
    public Guid Id { get; set; }
    
    public GetStudentByIdQuery(Guid id)
    {
        Id = id;
    }
}