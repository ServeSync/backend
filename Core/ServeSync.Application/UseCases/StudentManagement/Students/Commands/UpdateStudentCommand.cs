using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class UpdateStudentCommand : ICommand
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public bool Gender { get; set; }
    public DateTime Birth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ImageUrl { get; set; }
    public string CitizenId { get; set; }
    public string? Address { get; set; }
    public string? HomeTown { get; set; }
    public Guid HomeRoomId { get; set; }
    public Guid EducationProgramId { get; set; }
    
    public UpdateStudentCommand(
        Guid id,
        string code,
        string fullName, 
        bool gender, 
        DateTime birth, 
        string email, 
        string phone,
        string? address, 
        string imageUrl, 
        string citizenId,
        string? homeTown, 
        Guid homeRoomId, 
        Guid educationProgramId)
    {
        Id = id;
        Code = code;
        FullName = fullName;
        Gender = gender;
        Birth = birth;
        Email = email;
        Phone = phone;
        Address = address;
        ImageUrl = imageUrl;
        CitizenId = citizenId;
        HomeTown = homeTown;
        HomeRoomId = homeRoomId;
        EducationProgramId = educationProgramId;
    }
}