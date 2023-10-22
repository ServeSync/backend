using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.Common.Validations;

public class LessThanCurrentDateAttribute : ValidationAttribute
{
    private string FieldName { get; set; }
    
    public LessThanCurrentDateAttribute(string fieldName)
    {
        FieldName = fieldName;
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date && date > DateTime.Now)
        {
            return new ValidationResult($"{FieldName} must be less than current date!");
        }

        return ValidationResult.Success;
    }
}