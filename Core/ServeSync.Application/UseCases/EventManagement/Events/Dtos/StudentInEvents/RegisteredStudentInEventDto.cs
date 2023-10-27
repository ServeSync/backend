using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;

public class RegisteredStudentInEventDto : RegisteredStudentInEventRoleDto
{
    public string Role { get; set; } = null!;
}