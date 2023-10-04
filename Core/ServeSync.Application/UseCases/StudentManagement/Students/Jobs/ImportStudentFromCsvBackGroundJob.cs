using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Jobs;

public class ImportStudentFromCsvBackGroundJob : IBackGroundJob
{
    public IEnumerable<StudentCsvCreateDto> Students { get; set; }
    
    public ImportStudentFromCsvBackGroundJob(IEnumerable<StudentCsvCreateDto> students)
    {
        Students = students;
    }
}