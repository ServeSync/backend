using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class UpdateStudentByIdentityCommand : ICommand
{
    public string FullName { get; set; }
    public bool Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ImageUrl { get; set; }
    public string CitizenId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? HomeTown { get; set; }
    public string? Address { get; set; }

    public UpdateStudentByIdentityCommand(
        string fullName, 
        bool gender, 
        DateTime dateOfBirth, 
        string imageUrl,
        string citizenId, 
        string email, 
        string phone, 
        string? homeTown, 
        string? address)
    {
        FullName = fullName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        ImageUrl = imageUrl;
        CitizenId = citizenId;
        Email = email;
        Phone = phone;
        HomeTown = homeTown;
        Address = address;
    }
}