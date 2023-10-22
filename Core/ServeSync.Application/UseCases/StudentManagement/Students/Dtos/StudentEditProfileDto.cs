using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentEditProfileDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Phone]
    public string Phone { get; set; } = null!;
    public string? Address { get; set; }
    public string? HomeTown { get; set; }
    public string ImageUrl { get; set; } = null!;
}