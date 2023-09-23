using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Specifications;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.DomainServices;

public class EducationProgramDomainService : IEducationProgramDomainService
{
    private readonly IEducationProgramRepository _educationProgramRepository;

    public EducationProgramDomainService(IEducationProgramRepository educationProgramRepository)
    {
        _educationProgramRepository = educationProgramRepository;
    }
    
    public async Task<EducationProgram> CreateAsync(string name, int requiredActivityScore, int requiredCredit)
    {
        await CheckDuplicateNameAsync(name);
        
        var educationProgram = new EducationProgram(name, requiredActivityScore, requiredCredit);
        await _educationProgramRepository.InsertAsync(educationProgram);

        return educationProgram;
    }

    private async Task CheckDuplicateNameAsync(string name)
    {
        if (await _educationProgramRepository.AnyAsync(new EducationProgramByNameSpecification(name)))
        {
            throw new DuplicateEducationProgramException(name);
        }
    }
}