using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.ReadModels.Events;

public class EventReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string Introduction { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public EventStatus Status { get; set; }
    public EventType Type { get; set; }
    public int Rating { get; set; }

    public AuditReadModel Created { get; set; } = null!;
    public AuditReadModel Modified { get; set; } = null!;
    
    public EventAddressReadModel Address { get; set; } = null!;
    
    public EventActivityReadModel Activity { get; set; } = null!;
    public BasicEventOrganizationInEventReadModel RepresentativeOrganization { get; set; } = null!;
    
    public List<EventRoleReadModel> Roles { get; set; } = null!;
    public List<EventOrganizationInEventReadModel> Organizations { get; set; } = null!;
    public List<EventRegistrationInfoReadModel> RegistrationInfos { get; set; } = null!;
    public List<EventAttendanceInfoReadModel> AttendanceInfos { get; set; } = null!;
    
    public int Capacity => Roles.Sum(x => x.Quantity);
    public int Registered => Roles.Sum(x => x.Registered);
    public int ApprovedRegistered => Roles.Sum(x => x.ApprovedRegistered);
    public int Attended => AttendanceInfos.Sum(x => x.Attended);
}

public class EventAddressReadModel
{
    public string FullAddress { get; set; } = null!;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

public class AuditReadModel
{
    public string? Id { get; set; }
    public DateTime At { get; set; }
    public string? FullName { get; set; }
    public string? ImageUrl { get; set; }
}