using System.ComponentModel.DataAnnotations;
using ServeSync.Application.Common.Validations;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentUpdateDto
{
    [MinLength(9)]
    [MaxLength(9)]
    public string Code { get; set; } = null!;
    
    [MinLength(5)]
    public string FullName { get; set; } = null!;
    
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Phone]
    public string Phone { get; set; } = null!;
    
    [LessThanCurrentDate(nameof(Birth))]
    public DateTime Birth { get; set; }
    
    public bool Gender { get; set; }
    public string? Address { get; set; }
    public string? HomeTown { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string CitizenId { get; set; } = null!;
    
    public Guid HomeRoomId { get; set; }
    public Guid EducationProgramId { get; set; }
}