using Microsoft.AspNetCore.Http;
using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ImportStudentFromCsvCommand : ICommand
{
    public IFormFile File { get; set; }
    
    public ImportStudentFromCsvCommand(IFormFile file)
    {
        File = file;
    }
}