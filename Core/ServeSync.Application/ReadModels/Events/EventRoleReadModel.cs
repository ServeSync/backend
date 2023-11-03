using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.ReadModels.Events;

public class EventRoleReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsNeedApprove { get; set; }
    public double Score { get; set; }
    public int Quantity { get; set; }
    public int Registered => RegisteredStudents.Count;
    public int ApprovedRegistered => RegisteredStudents.Count(x => x.Status == EventRegisterStatus.Approved);
    public List<RegisteredStudentInEventRoleReadModel> RegisteredStudents { get; set; } = new();
}

public class RegisteredStudentInEventRoleReadModel : BaseReadModel<Guid>
{
    public Guid StudentId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public EventRegisterStatus Status { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string HomeRoomName { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
    public string IdentityId { get; set; } = null!;
}

public class RegisteredStudentInEventReadModel : RegisteredStudentInEventRoleReadModel
{
    public string Role { get; set; } = null!;
}