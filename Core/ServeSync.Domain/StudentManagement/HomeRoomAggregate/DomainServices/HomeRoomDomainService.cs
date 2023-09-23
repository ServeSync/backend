using ServeSync.Domain.StudentManagement.FacultyAggregate;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.DomainServices;

public class HomeRoomDomainService : IHomeRoomDomainService
{
    private readonly IHomeRoomRepository _homeRoomRepository;
    private readonly IFacultyRepository _facultyRepository;
    
    public HomeRoomDomainService(IHomeRoomRepository homeRoomRepository, IFacultyRepository facultyRepository)
    {
        _homeRoomRepository = homeRoomRepository;
        _facultyRepository = facultyRepository;
    }
    
    public async Task<HomeRoom> CreateAsync(string name, Guid facultyId)
    {
        await CheckFacultyExistsAsync(facultyId);
        await CheckDuplicateNameAsync(name);

        var faculty = new HomeRoom(name, facultyId);
        await _homeRoomRepository.InsertAsync(faculty);

        return faculty;
    }

    private async Task CheckDuplicateNameAsync(string name)
    {
        if (await _homeRoomRepository.AnyAsync(new HomeRoomByNameSpecification(name)))
        {
            throw new DuplicateHomeRoomException(name);
        }
    }
    
    private async Task CheckFacultyExistsAsync(Guid facultyId)
    {
        if (!await _facultyRepository.IsExistingAsync(facultyId))
        {
            throw new FacultyNotFoundException(facultyId);
        }
    }
}