using ServeSync.Application.Common.Dtos;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;

public class RegisteredStudentFilterRequestDto : PagingRequestDto
{
    public EventRegisterStatus? Status { get; set; }
}