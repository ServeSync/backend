using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Specifications;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.DomainServices;

public class FacultyDomainService : IFacultyDomainService
{
    private readonly IFacultyRepository _facultyRepository;
    
    public FacultyDomainService(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }
    
    public async Task<Faculty> CreateAsync(string name)
    {
        await CheckDuplicateNameAsync(name);

        var faculty = new Faculty(name);
        await _facultyRepository.InsertAsync(faculty);

        return faculty;
    }

    private async Task CheckDuplicateNameAsync(string name)
    {
        if (await _facultyRepository.AnyAsync(new FacultyByNameSpecification(name)))
        {
            throw new DuplicateFacultyException(name);
        }
    }
}