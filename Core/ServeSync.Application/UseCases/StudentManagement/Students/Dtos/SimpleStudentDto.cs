namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class SimpleStudentDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
}