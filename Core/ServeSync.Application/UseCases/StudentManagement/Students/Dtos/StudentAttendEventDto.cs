using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentAttendEventDto
{
    [Required]
    public string Code { get; set; } = null!;
    
    [Required]
    public double Latitude { get; set; }
    
    [Required]
    public double Longitude { get; set; }
}