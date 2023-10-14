using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> FindByIdentityAsync(string identityId);
}