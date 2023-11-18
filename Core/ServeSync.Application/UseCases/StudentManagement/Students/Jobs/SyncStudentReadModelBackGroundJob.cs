using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Jobs;

public class SyncStudentReadModelBackGroundJob : IBackGroundJob
{
    public Guid StudentId { get; set; }
    
    public SyncStudentReadModelBackGroundJob(Guid studentId)
    {
        StudentId = studentId;
    }
}