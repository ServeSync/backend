using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.FacultyAggregate;
using ServeSync.Domain.StudentManagement.FacultyAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.DomainServices;
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
            "Nhiệt - Điện lạnh",
            "Cơ khí Giao thông",
            "Công nghệ Thông tin",
            "Điện tử - Viễn thông",
            "Xây dựng Cầu - Đường",
            "Xây dựng Thủy lợi - Thủy Điện",
            "Xây dựng Dân dụng và Công nghiệp",
            "Khoa học Công nghệ tiên tiến"
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
            var homeRoomsByFaculty = GetHomeRooms();

            foreach (var facultyName in homeRoomsByFaculty.Keys)
            {
                var faculty = await _facultyRepository.FindAsync(new FacultyByNameSpecification(facultyName));

                if (faculty == null)
                {
                    continue;
                }

                foreach (var homeRoomName in homeRoomsByFaculty[facultyName])
                {
                    await _homeRoomDomainService.CreateAsync(homeRoomName, faculty.Id);
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
        // if (await _studentRepository.AnyAsync())
        // {
        //     _logger.LogInformation("Student data has been seeded!");
        //     return;
        // }
        //
        // _logger.LogInformation("Seeding student data...");
        //
        // try
        // {
        //     var homerooms = await _homeRoomRepository.FindAllAsync();
        //     var educationPrograms = await _educationProgramRepository.FindAllAsync();
        //
        //     await _unitOfWork.BeginTransactionAsync();
        //     
        //     foreach (var homeroom in homerooms)
        //     {
        //         for (var i = 0; i < 10; i++)
        //         {
        //             var faker = new Faker();
        //             
        //             var fullName = faker.Person.FullName;
        //             var email = $"{faker.Random.AlphaNumeric(9)}@gmail.com";
        //             var code = faker.Random.AlphaNumeric(8);
        //             
        //             await _studentDomainService.CreateAsync(
        //                 code,
        //                 fullName,
        //                 faker.Random.Bool(),
        //                 faker.Person.DateOfBirth,
        //                 faker.Person.Avatar,
        //                 faker.Random.AlphaNumeric(8),
        //                 email,
        //                 faker.Person.Phone,
        //                 homeroom.Id,
        //                 educationPrograms[faker.Random.Int(0, educationPrograms.Count - 1)].Id,
        //                 faker.Address.City(),
        //                 faker.Address.FullAddress());
        //         }
        //     }
        //     
        //     await _unitOfWork.CommitTransactionAsync(false);
        //
        //     _logger.LogInformation("Seeded students data successfully!");
        // }
        // catch (Exception e)
        // {
        //     await _unitOfWork.CommitTransactionAsync(true);
        //     _logger.LogError("Seeding students data failed: {Message}", e.Message);
        // }
    }

    private IDictionary<string, string[]> GetHomeRooms()
    {
        var dictionary = new Dictionary<string, string[]>();
            
        dictionary.Add("Cơ khí Giao thông", new string[]
        {
            "19C4CLC1", "19C4CLC2", "19C4CLC3", "19C4CLC4", "19HTCN", "19KTTT", 
            "20C4A", "20C4B", "20C4CLC4", "20HTCN", "20KTTT", "21C4A", "21C4CLC2", 
            "21HTCN", "21HTCN2", "21KTOTO1", "21KTOTO2", "22C4C", "22KTOTO1", "22KTOTO2"
        });
        
        dictionary.Add("Công nghệ Thông tin", new string[]
        {
            "19TCLC_DT1", "19TCLC_DT2", "19TCLC_DT3", "19TCLC_DT4", "19TCLC_DT5", "19TCLC_DT6", 
            "19TCLC_Nhat1", "19TCLC_Nhat2", "20T1", "20T2", "20TCLC_DT1", "20TCLC_DT2", "20TCLC_DT3", 
            "20TCLC_DT4", "20TCLC_DT5", "20TCLC_KHDL", "20TCLC_NHAT1", "20TCLC_NHAT2", "21T_DT", 
            "21T_DT2", "21TCLC_DT1", "21TCLC_DT2", "21TCLC_DT3", "21TCLC_DT4", "21TCLC_KHDL", 
            "21TCLC_Nhat1", "21TCLC_Nhat2", "22T_DT1", "22T_DT2", "22T_DT3", "22T_DT4", "22T_DT5", 
            "22T_KHDL", "22T_Nhat1", "22T_Nhat2"
        });
        
        dictionary.Add("Cơ khí", new string[]
        {
            "19C1A", "19C1B", "19C1C", "19C1D", "19CDTCLC1", "19CDTCLC2", "19CDTCLC3", "19CDTCLC4", 
            "20C1A", "20C1C", "20CDT1", "20CDT2", "20CDTCLC1", "20CDTCLC2", "20CKHK", "21C1A", "21C1B", 
            "21C1C", "21C1D", "21CDT1", "21CDT2", "21CDTCLC1", "21CDTCLC2", "21CKHK", "22C1A", "22CDT2", "22CDT3"
        });
        
        dictionary.Add("Hóa", new string[]
        {
            "19H2CLC1", "19H2CLC2", "19H5CLC", "19KTHH1", "19KTHH2", "19SH1", "19SH2", "20H2", 
            "20H2CLC", "20H5", "20KTHH1", "20KTHH2", "20SH1", "20SH2", "21H2", "21H2B", "21H2CLC1", 
            "21H2CLC2", "21H5", "21KTHH1", "21KTHH2", "21SH1", "21SH2", "22H2B", "22H2C", "22H5", 
            "22KTHH1", "22KTHH2", "22SH1", "22SHYD"
        });
        
        dictionary.Add("Điện", new string[]
        {
            "19DCLC2", "19DCLC3", "19DCLC4", "19TDHCLC1", "19TDHCLC2", "19TDHCLC3", "19TDHCLC4", 
            "19TDHCLC5", "20D1", "20D2", "20DCLC1", "20DCLC2", "20DCLC3", "20DCLC4", "20TDH1", 
            "20TDH2", "20TDHCLC1", "20TDHCLC2", "20TDHCLC3", "20TDHCLC4", "21D1", "21D2", "21DCLC1", 
            "21DCLC4", "21TDH1", "21TDH2", "21TDHCLC1", "21TDHCLC2", "22D2", "22D4", "22TDH1", "22TDH2", 
            "22TDH3", "22TDH4"
        });
        
        dictionary.Add("Điện tử - Viễn thông", new string[]
        {
            "19DTCLC1", "19DTCLC2", "19DTCLC3", "19DTCLC4", "20DT1", "20DTCLC1", "20DTCLC2", "20KTMT1", 
            "20KTMT2", "21DT1", "21DT2", "21DTCLC1", "21DTCLC3", "21DTCLC4", "21KTMT", "21KTMT2", "22DT1", 
            "22DT2", "22DT3", "22DT4", "22DT5", "22KTMT1", "22KTMT2"
        });

        dictionary.Add("Kiến trúc", new string[]
        {
            "19KTCLC1", "19KTCLC2", "20KT", "20KTCLC", "21KT", "21KT2", "21KTCLC", "22KT1", "22KT2"
        });
        
        dictionary.Add("Môi trường", new string[]
        {
            "19QLMT", "20MT", "20QLMT", "21MT", "22QLMT"
        });
        
        dictionary.Add("Xây dựng Dân dụng và Công nghiệp", new string[]
        {
            "19X1CLC2", "19X1CLC3", "20X1A", "20X1B", "20X1CLC1", "20X1CLC2", "21X1A", "21X1CLC1", "21X1CLC2", "22X1A", "22X1B", "22X1C"
        });
        
        dictionary.Add("Xây dựng Thủy lợi - Thủy Điện", new string[]
        {
            "19THXD", "20THXD1", "20X2", "21THXD1", "21THXD2", "22THXD", "22X2"
        });
        
        dictionary.Add("Xây dựng Cầu - Đường", new string[]
        {
            "19CSHT", "19VLXD", "19X3CLC", "21CSHT", "21X3", "21X3B", "22BIM_AI"
        });
        
        dictionary.Add("Quản lý dự án", new string[]
        {
            "19KXCLC1", "19KXCLC2", "19QLCN1", "19QLCN2", "20KX", "20KXCLC", "20QLCN1", "20QLCN2", "21KX", "21KXCLC", 
            "21QLCN1", "21QLCN2", "22KX1", "22KX2", "22QLCN1", "22QLCN2"
        });
        
        dictionary.Add("Nhiệt - Điện lạnh", new string[]
        {
            "19NCLC", "20N", "21N", "21NCLC"
        });
        
        dictionary.Add("Khoa học Công nghệ tiên tiến", new string[]
        {
            "18PFIEV1", "18PFIEV2", "18PFIEV3", "19ECE", "19ES", "19PFIEV2", "19PFIEV3", "20ECE", "20ES", "20PFIEV3", 
            "21ECE", "21ES", "21PFIEV1", "22ECE", "22ES", "22PFIEV1", "22PFIEV2"
        });

        return dictionary;
    }
}