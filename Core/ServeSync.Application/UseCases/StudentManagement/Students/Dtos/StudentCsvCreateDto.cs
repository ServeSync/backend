using CsvHelper.Configuration.Attributes;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentCsvCreateDto
{
    [Name("MSSV")]
    public string Code { get; set; } = null!;
    
    [Name("FullName")]
    public string FullName { get; set; } = null!;
    
    [Name("Email")]
    public string Email { get; set; } = null!;
    
    [Name("Phone")]
    public string Phone { get; set; } = null!;
    
    [Name("Birth")]
    public DateTime Birth { get; set; }
    
    [Name("Gender")]
    public bool Gender { get; set; }
    
    [Name("Address")]
    public string? Address { get; set; }
    
    [Name("HomeTown")]
    public string? HomeTown { get; set; }
    
    [Name("ImageUrl")]
    public string ImageUrl { get; set; } = null!;
    
    [Name("CitizenId")]
    public string CitizenId { get; set; } = null!;

    [Name("HomeRoom")] 
    public string HomeRoom { get; set; } = null!;

    [Name("EducationProgram")] 
    public string EducationProgram { get; set; } = null!;
}