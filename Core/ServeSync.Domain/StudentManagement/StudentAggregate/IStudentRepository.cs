using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> FindByIdentityAsync(string identityId);
    
    Task<IQueryable<StudentEventRegister>> GetEventStudentRegisteredQueryable(ISpecification<StudentEventRegister, Guid> specification);
    
    Task<IQueryable<StudentEventAttendance>> GetEventStudentAttendanceQueryable(ISpecification<StudentEventAttendance, Guid> specification);
}