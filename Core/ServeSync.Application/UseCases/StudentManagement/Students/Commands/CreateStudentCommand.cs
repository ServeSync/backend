using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class CreateStudentCommand : ICommand<Guid>
{
    public string Code { get; set; }
    public string FullName { get; set; }
    public bool Gender { get; set; }
    public DateTime Birth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string ImageUrl { get; set; }
    public string? HomeTown { get; set; }
    public string CitizenId { get; set; }
    public Guid HomeRoomId { get; set; }
    public Guid EducationProgramId { get; set; }

    public CreateStudentCommand(
        string code,
        string fullName, 
        bool gender, 
        DateTime birth, 
        string email, 
        string phone,
        string? address, 
        string imageUrl, 
        string? homeTown, 
        string citizenId, 
        Guid homeRoomId, 
        Guid educationProgramId)
    {
        Code = code;
        FullName = fullName;
        Gender = gender;
        Birth = birth;
        Email = email;
        Phone = phone;
        Address = address;
        ImageUrl = imageUrl;
        HomeTown = homeTown;
        CitizenId = citizenId;
        HomeRoomId = homeRoomId;
        EducationProgramId = educationProgramId;
    }
}