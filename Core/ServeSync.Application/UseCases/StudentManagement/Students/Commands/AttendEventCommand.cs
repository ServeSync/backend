using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class AttendEventCommand : ICommand
{
    public Guid EventId { get; set; }
    public string Code { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    
    public AttendEventCommand(Guid eventId, string code, double longitude, double latitude)
    {
        EventId = eventId;
        Code = code;
        Longitude = longitude;
        Latitude = latitude;
    }
}