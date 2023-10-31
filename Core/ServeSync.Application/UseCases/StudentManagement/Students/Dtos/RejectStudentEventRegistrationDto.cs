using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class RejectStudentEventRegistrationDto
{
    [Required] 
    [MinLength(5)] 
    public string RejectReason { get; set; } = null!;
}