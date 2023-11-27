using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

public class BasicEventRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsNeedApprove { get; set; }
    public double Score { get; set; }
    public int Quantity { get; set; }
}

public class EventRoleDto : BasicEventRoleDto
{
    public EventRegisterStatus? Status { get; set; }
    public bool IsRegistered { get; set; }
    public int Registered { get; set; }
    public int ApprovedRegistered { get; set; }
}

public class EventRoleDetailDto : BasicEventRoleDto
{
    public List<RegisteredStudentInEventRoleDto> RegisteredStudents { get; set; } = null!;
}

public class RegisteredStudentInEventRoleDto 
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid StudentId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Description { get; set; }
    public string? RejectReason { get; set; } = null!;
    public EventRegisterStatus Status { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string HomeRoomName { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
}