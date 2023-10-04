using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class UpdateStudentByIdentityCommand : ICommand
{
    public string ImageUrl { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? HomeTown { get; set; }
    public string? Address { get; set; }

    public UpdateStudentByIdentityCommand(
        string imageUrl,
        string email, 
        string phone, 
        string? homeTown, 
        string? address)
    {
        ImageUrl = imageUrl;
        Email = email;
        Phone = phone;
        HomeTown = homeTown;
        Address = address;
    }
}