using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.DomainServices;

public interface IFacultyDomainService
{
    Task<Faculty> CreateAsync(string name);
}