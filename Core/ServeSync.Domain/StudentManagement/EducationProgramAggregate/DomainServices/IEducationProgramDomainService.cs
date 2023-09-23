using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.DomainServices;

public interface IEducationProgramDomainService
{
    Task<EducationProgram> CreateAsync(string name, int requiredActivityScore, int requiredCredit);
}