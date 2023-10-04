namespace ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;

public class EducationProgramDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int RequiredActivityScore { get; set; }
    public int RequiredCredit { get; set; }
}