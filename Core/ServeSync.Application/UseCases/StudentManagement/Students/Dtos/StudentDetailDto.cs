using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentDetailDto : BasicStudentDto
{
    public FacultyDto Faculty { get; set; } = null!;
    public HomeRoomDto HomeRoom { get; set; } = null!;
    public EducationProgramDto EducationProgram { get; set; } = null!;
}