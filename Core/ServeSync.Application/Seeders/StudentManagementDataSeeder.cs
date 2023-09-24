using Bogus;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.FacultyAggregate;
using ServeSync.Domain.StudentManagement.FacultyAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

namespace ServeSync.Application.Seeders;

public class StudentManagementDataSeeder : IDataSeeder
{
    private readonly IFacultyRepository _facultyRepository;
    private readonly IHomeRoomRepository _homeRoomRepository;
    private readonly IEducationProgramRepository _educationProgramRepository;
    private readonly IStudentRepository _studentRepository;
    
    private readonly IEducationProgramDomainService _educationProgramDomainService;
    private readonly IFacultyDomainService _facultyDomainService;
    private readonly IHomeRoomDomainService _homeRoomDomainService;
    private readonly IStudentDomainService _studentDomainService;

    private readonly IIdentityService _identityService;

    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public StudentManagementDataSeeder(
        IFacultyRepository facultyRepository,
        IHomeRoomRepository homeRoomRepository,
        IEducationProgramRepository educationProgramRepository,
        IStudentRepository studentRepository,
        IEducationProgramDomainService educationProgramDomainService,
        IFacultyDomainService facultyDomainService,
        IHomeRoomDomainService homeRoomDomainService,
        IStudentDomainService studentDomainService,
        IIdentityService identityService,
        ILogger<StudentManagementDataSeeder> logger,
        IUnitOfWork unitOfWork)
    {
        _facultyRepository = facultyRepository;
        _homeRoomRepository = homeRoomRepository;
        _educationProgramRepository = educationProgramRepository;
        _studentRepository = studentRepository;
        _educationProgramDomainService = educationProgramDomainService;
        _facultyDomainService = facultyDomainService;
        _homeRoomDomainService = homeRoomDomainService;
        _studentDomainService = studentDomainService;
        _identityService = identityService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task SeedAsync()
    {
        await SeedFacultyAsync();
        await SeedHomeRoomsAsync();
        await SeedEducationProgramsAsync();
        await SeedStudentsAsync();
    }

    private async Task SeedFacultyAsync()
    {
        if (await _facultyRepository.AnyAsync())
        {
            _logger.LogInformation("Faculty data has been seeded!");
            return;
        }
        
        _logger.LogInformation("Seeding faculty data...");
        
        var facultyNames = new List<string>
        {
            "Hóa",
            "Điện",
            "Cơ khí",
            "Kiến trúc",
            "Môi trường",
            "Quản lý dự án",
            "Khoa Nhiệt - Điện lạnh",
            "Khoa Cơ khí Giao thông",
            "Khoa Công nghệ Thông tin",
            "Khoa Điện tử - Viễn thông",
            "Khoa Xây dựng Cầu - Đường",
            "Khoa Xây dựng Thủy lợi - Thủy Điện",
            "Khoa Xây dựng Dân dụng và Công nghiệp",
            "Khoa Khoa học Công nghệ tiên tiến"
        };

        try
        {
            foreach (var name in facultyNames)
            {
                var faculty = await _facultyDomainService.CreateAsync(name);
            }

            await _unitOfWork.CommitAsync();
        
            _logger.LogInformation("Seeded faculty data successfully!");
        }
        catch (Exception e)
        {
            _logger.LogError("Seeding faculty data failed: {Message}", e.Message);
        }
    }

    private async Task SeedHomeRoomsAsync()
    {
        if (await _homeRoomRepository.AnyAsync())
        {
            _logger.LogInformation("Home room data has been seeded!");
            return;
        }
        
        _logger.LogInformation("Seeding home room data...");
        
        try
        {
            var faker = new Faker();
            var faculties = await _facultyRepository.FindAllAsync();

            foreach (var faculty in faculties)
            {
                for (var i = 0; i < 5; i++)
                {
                    await _homeRoomDomainService.CreateAsync($"{faker.Random.Word()} - {faker.Random.AlphaNumeric(3)}", faculty.Id);
                }    
            }

            await _unitOfWork.CommitAsync();
        
            _logger.LogInformation("Seeded homerooms data successfully!");
        }
        catch (Exception e)
        {
            _logger.LogError("Seeding homerooms data failed: {Message}", e.Message);
        }
    }
    
    private async Task SeedEducationProgramsAsync()
    {
        if (await _educationProgramRepository.AnyAsync())
        {
            _logger.LogInformation("Education program data has been seeded!");
            return;
        }
        
        _logger.LogInformation("Seeding education program data...");
        
        try
        {
            await _educationProgramDomainService.CreateAsync("Cử nhân", 60, 130);
            await _educationProgramDomainService.CreateAsync("Kỹ sư", 75, 150);
            
            
            await _unitOfWork.CommitAsync();
        
            _logger.LogInformation("Seeded education program data successfully!");
        }
        catch (Exception e)
        {
            _logger.LogError("Seeding education program data failed: {Message}", e.Message);
        }
    }

    private async Task SeedStudentsAsync()
    {
        if (await _studentRepository.AnyAsync())
        {
            _logger.LogInformation("Student data has been seeded!");
            return;
        }
        
        _logger.LogInformation("Seeding student data...");
        
        try
        {
            var homerooms = await _homeRoomRepository.FindAllAsync();
            var educationPrograms = await _educationProgramRepository.FindAllAsync();

            await _unitOfWork.BeginTransactionAsync();
            
            foreach (var homeroom in homerooms)
            {
                for (var i = 0; i < 10; i++)
                {
                    var faker = new Faker();
                    
                    var fullName = faker.Person.FullName;
                    var email = $"{faker.Random.AlphaNumeric(9)}@gmail.com";
                    var code = faker.Random.AlphaNumeric(8);
                    
                    var result = await _identityService.CreateUserAsync(
                        fullName,
                        code,
                        email,
                        "Student");

                    if (result.IsSuccess)
                    {
                        await _studentDomainService.CreateAsync(
                            code,
                            fullName,
                            faker.Random.Bool(),
                            faker.Person.DateOfBirth,
                            faker.Person.Avatar,
                            faker.Random.AlphaNumeric(8),
                            email,
                            faker.Person.Phone,
                            homeroom.Id,
                            educationPrograms[faker.Random.Int(0, educationPrograms.Count - 1)].Id,
                            result.Data.Id,
                            faker.Address.City(),
                            faker.Address.FullAddress());    
                    }
                }    
            }

            await _unitOfWork.CommitTransactionAsync(true);
        
            _logger.LogInformation("Seeded students data successfully!");
        }
        catch (Exception e)
        {
            await _unitOfWork.CommitTransactionAsync(true);
            _logger.LogError("Seeding students data failed: {Message}", e.Message);
        }
    }
}