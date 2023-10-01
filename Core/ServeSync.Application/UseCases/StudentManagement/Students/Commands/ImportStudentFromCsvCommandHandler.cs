using System.Globalization;
using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Http;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Jobs;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ImportStudentFromCsvCommandHandler : ICommandHandler<ImportStudentFromCsvCommand>
{
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public ImportStudentFromCsvCommandHandler(IBackGroundJobManager backGroundJobManager)
    {
        _backGroundJobManager = backGroundJobManager;
    }
    
    public Task Handle(ImportStudentFromCsvCommand request, CancellationToken cancellationToken)
    {
        var students = GetStudentsFromFile(request.File);
        _backGroundJobManager.Fire(new ImportStudentFromCsvBackGroundJob(students));
        return Task.CompletedTask;
    }
    
    private IEnumerable<StudentCsvCreateDto> GetStudentsFromFile(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        try
        {
            return csvReader.GetRecords<StudentCsvCreateDto>().ToList();
        }
        catch (TypeConverterException e)
        {
            throw new ResourceInvalidDataException($"Could not convert {e.Text} to appropriate type!");
        }
        catch (HeaderValidationException e)
        {
            throw new ResourceInvalidDataException("Invalid csv format!");
        }
    }
}