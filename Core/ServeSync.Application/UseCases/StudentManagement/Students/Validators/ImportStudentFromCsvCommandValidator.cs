using FluentValidation;
using Microsoft.AspNetCore.Http;
using ServeSync.Application.UseCases.StudentManagement.Students.Commands;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Validators;

public class ImportStudentFromCsvCommandValidator : AbstractValidator<ImportStudentFromCsvCommand>
{
    public ImportStudentFromCsvCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .Must(HaveValidContentType)
            .WithMessage("Invalid content type, file must be an csv!");
    }

    private static bool HaveValidContentType(IFormFile form)
    {
        if (!string.Equals(form.ContentType, "text/csv", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        
        var postedFileExtension = Path.GetExtension(form.FileName);
        if (!string.Equals(postedFileExtension , ".csv", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}