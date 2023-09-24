namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class FlatStudentDto : BasicStudentDto
{
    public Guid HomeRoomId { get; set; }
    public Guid EducationProgramId { get; set; }
}