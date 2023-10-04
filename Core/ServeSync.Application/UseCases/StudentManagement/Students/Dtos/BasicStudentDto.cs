namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class BasicStudentDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? HomeTown { get; set; }
    public string? Address { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string CitizenId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public string IdentityId { get; set; } = null!;
}