﻿using Bogus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.Seeders;

public class EventManagementDataSeeder : IDataSeeder
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IEventCollaborationRequestDomainService _eventCollaborationRequestDomainService;
    private readonly IEventCategoryDomainService _eventCategoryDomainService;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IEventDomainService _eventDomainService;
    private readonly IProofDomainService _proofDomainService;
    
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IProofRepository _proofRepository;
    private readonly IEventCollaborationRequestRepository _eventCollaborationRequestRepository;
    private readonly IBasicReadOnlyRepository<StudentEventRegister, Guid> _studentEventRegisterRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    private readonly IBasicReadOnlyRepository<StudentEventAttendance, Guid> _studentAttendanceRepository;

    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public EventManagementDataSeeder(
        IStudentDomainService studentDomainService,
        IEventCategoryDomainService eventCategoryDomainService,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IEventDomainService eventDomainService,
        IEventCollaborationRequestDomainService eventCollaborationRequestDomainService,
        IProofDomainService proofDomainService,
        IEventCategoryRepository eventCategoryRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        IEventRepository eventRepository,
        IStudentRepository studentRepository,
        IProofRepository proofRepository,
        IEventCollaborationRequestRepository eventCollaborationRequestRepository,
        IBasicReadOnlyRepository<StudentEventRegister, Guid> studentEventRegisterRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        IBasicReadOnlyRepository<StudentEventAttendance, Guid> studentAttendanceRepository,
        IHostEnvironment hostEnvironment,
        ILogger<StudentManagementDataSeeder> logger, 
        IUnitOfWork unitOfWork)
    {
        _studentDomainService = studentDomainService;
        _eventCategoryDomainService = eventCategoryDomainService;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _eventDomainService = eventDomainService;
        _eventCategoryRepository = eventCategoryRepository;
        _proofDomainService = proofDomainService;
        _eventCollaborationRequestDomainService = eventCollaborationRequestDomainService;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventRepository = eventRepository;
        _studentRepository = studentRepository;
        _proofRepository = proofRepository;
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _studentEventRegisterRepository = studentEventRegisterRepository;
        _eventActivityRepository = eventActivityRepository;
        _studentAttendanceRepository = studentAttendanceRepository;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        await SeedEventCategoriesAsync();
        
        if (_hostEnvironment.IsDevelopment())
        {
            await SeedEventOrganizationsAsync();
            await SeedEventsAsync();
            await SeedRegisterEventAsync();
            await SeedInternalProofAsync();
            await SeedExternalProofAsync();
            await SeedAttendanceEventsForStudentAsync();
            await SeedEventCollaborationRequestsAsync();
        }
    }

    private async Task SeedEventCategoriesAsync()
    {
        if (await _eventCategoryRepository.AnyAsync())
        {
            _logger.LogInformation("Event categories already seeded.");
            return;
        }

        try
        {
            var eventCategories = GetEventCategories();
            var index = 0;
            foreach (var eventCategoryData in eventCategories)
            {
                var eventCategory = await _eventCategoryDomainService.CreateAsync(eventCategoryData.Key.Name, index++, eventCategoryData.Key.Type);

                foreach (var activity in eventCategoryData.Value)
                {
                    _eventCategoryDomainService.AddActivity(eventCategory, activity.Name, activity.MinScore, activity.MaxScore);
                }
            }
        
            await _unitOfWork.CommitAsync();
        
            _logger.LogInformation("Seeded event categories successfully!");
        }
        catch (Exception e)
        {
            _logger.LogInformation("Seeded event categories failed: {Message}", e.Message);
        }
    }

    private async Task SeedEventOrganizationsAsync()
    {
        if (await _eventOrganizationRepository.AnyAsync())
        {
            _logger.LogInformation("Event organizations already seeded.");
            return;
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            
            for (var i = 0; i < 5; i++)
            {
                var faker = new Faker();
                var eventOrganization = await _eventOrganizationDomainService.CreateAsync(
                    faker.Company.CompanyName(),
                    faker.Internet.Email(),
                    faker.Phone.PhoneNumber(),
                    faker.Image.PicsumUrl(),
                    faker.Lorem.Sentence(),
                    faker.Address.FullAddress(),
                    OrganizationStatus.Active);
                
                await _eventOrganizationRepository.InsertAsync(eventOrganization);

                for (var j = 0; j < 5; j++)
                {
                    faker = new Faker();
                    _eventOrganizationDomainService.AddContact(
                        eventOrganization,
                        faker.Person.FullName,
                        faker.Person.Email,
                        faker.Person.Phone,
                        faker.Person.Avatar,
                        faker.Random.Bool(),
                        faker.Person.DateOfBirth,
                        faker.Person.Address.City,
                        faker.Commerce.Department(),
                        OrganizationStatus.Active);
                }
            }
        
            await _unitOfWork.CommitTransactionAsync();
        
            _logger.LogInformation("Seeded event organizations successfully!");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogInformation("Seeded event organizations failed: {Message}", e.Message);
        }
    }
    
    private async Task SeedEventsAsync()
    {
        if (await _eventRepository.AnyAsync())
        {
            _logger.LogInformation("Events already seeded.");
            return;
        }
        
        var eventOrganizations = await _eventOrganizationRepository.FindAllAsync();
        var eventActivities = await _eventActivityRepository.FindAllAsync(x => x.EventCategory.Type == EventCategoryType.Event);
       
        for (var i = 0; i < 100; i++)
        {
            try
            {
                var dateTime = DateTime.UtcNow;
                var faker = new Faker();
                await _unitOfWork.BeginTransactionAsync();

                var eventActivity = faker.PickRandom(eventActivities);
                var @event = await _eventDomainService.CreateAsync(
                    faker.Person.FullName,
                    faker.Lorem.Sentence(),
                    faker.Lorem.Paragraph(),
                    faker.Image.PicsumUrl(),
                    faker.PickRandom<EventType>(),
                    faker.Date.Between(DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddDays(2)),
                    faker.Date.Between(DateTime.UtcNow.AddDays(0), DateTime.UtcNow.AddDays(2)),
                    eventActivity.Id,
                    faker.Address.FullAddress(),
                    faker.Random.Double(-180, 180),
                    faker.Random.Double(-90, 90));

                for (var j = 0; j < 3; j++)
                {
                    faker = new Faker();
                    await _eventDomainService.AddRoleAsync(
                        @event,
                        faker.Name.JobTitle(),
                        faker.Lorem.Sentence(),
                        faker.Random.Bool(),
                        eventActivity.MinScore,
                        faker.Random.Int(1, 10),
                        dateTime);
                }

                _eventDomainService.AddAttendanceInfo(
                    @event,
                    faker.Date.Between(DateTime.UtcNow.AddMinutes(30), DateTime.UtcNow.AddDays(1)),
                    faker.Date.Between(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
                    dateTime);

                _eventDomainService.AddRegistrationInfo(
                    @event,
                    faker.Date.Between(DateTime.UtcNow.AddMinutes(30), DateTime.UtcNow.AddMinutes(45)),
                    faker.Date.Between(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(3)),
                    DateTime.UtcNow);

                var organization = faker.PickRandom(eventOrganizations);
                
                _eventDomainService.AddOrganization(
                    @event,
                    organization,
                    faker.Name.JobTitle(),
                    dateTime);

                _eventDomainService.AddRepresentative(
                    @event,
                    organization,
                    faker.PickRandom(organization.Contacts),
                    faker.Name.JobTitle(),
                    dateTime);
                
                @event.Approve(DateTime.UtcNow);

                await _eventRepository.InsertAsync(@event);
                await _unitOfWork.CommitAsync();

                _eventDomainService.SetRepresentativeOrganization(
                    @event,
                    faker.PickRandom(@event.Organizations).OrganizationId,
                    dateTime);

                _eventRepository.Update(@event);
                await _unitOfWork.CommitTransactionAsync(false);
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogInformation("Seeded event failed: {Message}", e.Message);
            }
        }
        
        _logger.LogInformation("Seeded events successfully!");
    }

    private async Task SeedRegisterEventAsync()
    {
        if (await _studentEventRegisterRepository.AnyAsync())
        {
            _logger.LogInformation("Student event registrations already seeded.");
            return;
        }
        
        if (!await _eventRepository.AnyAsync())
        {
            throw new Exception("Events not seeded yet.");
        }
        
        if (!await _studentRepository.AnyAsync())
        {
            throw new Exception("Students not seeded yet.");
        }
        
        var events = await _eventRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();
        foreach (var @event in events)
        {
            try
            {
                foreach (var student in students)
                {
                    var faker = new Faker();
                    await _studentDomainService.RegisterEventAsync(
                        student,
                        faker.PickRandom(@event.Roles.Where(x => !x.IsNeedApprove)).Id,
                        "Đăng ký tham gia sự kiện",
                        @event.RegistrationInfos.First().StartAt.AddMinutes(1));
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Seeded student event registrations failed: {Message}", e.Message);
            }
        }
        
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Seeded student event registrations success!");
    }

    private async Task SeedAttendanceEventsForStudentAsync()
    {
        if (await _studentAttendanceRepository.AnyAsync())
        {
            _logger.LogInformation("Student event attendances already seeded.");
            return;
        }
        
        if (!await _eventRepository.AnyAsync())
        {
            throw new Exception("Events not seeded yet.");
        }
        
        if (!await _studentRepository.AnyAsync())
        {
            throw new Exception("Students not seeded yet.");
        }

        if (!await _studentEventRegisterRepository.AnyAsync())
        {
            throw new Exception("Student event registrations not seeded yet.");
        }
        
        var events = await _eventRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();
        foreach (var @event in events.Take(events.Count / 2))
        {
            var faker = new Faker();
            foreach (var student in students)
            {
                try
                {
                    var attendance = faker.PickRandom(@event.AttendanceInfos);
                    await _studentDomainService.AttendEventAsync(
                        student,
                        @event.Id,
                        attendance.Code,
                        attendance.StartAt.AddMinutes(1),
                        @event.Address.Longitude,
                        @event.Address.Latitude);

                    _studentRepository.Update(student);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Seeded student event attendances failed: {Message}", e.Message);
                }
            }
        }
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Seeded student event attendances successfully!");
    }
    
    private async Task SeedEventCollaborationRequestsAsync()
    {
        if (await _eventCollaborationRequestRepository.AnyAsync())
        {
            _logger.LogInformation("Event collaboration requests already seeded.");
            return;
        }

        var activities = await _eventActivityRepository.FindAllAsync();

        for (var i = 0; i < 15; i++)
        {
            var faker = new Faker();
            try
            {
                var collaborationRequest = await _eventCollaborationRequestDomainService.CreateAsync(
                    faker.Name.FullName(),
                    faker.Lorem.Sentence(),
                    faker.Lorem.Paragraph(),
                    faker.Random.Int(1, 100),
                    faker.Image.PicsumUrl(),
                    faker.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                    faker.Date.Between(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
                    faker.PickRandom<EventType>(),
                    faker.PickRandom(activities).Id,
                    faker.Address.FullAddress(),
                    faker.Random.Double(-180, 180),
                    faker.Random.Double(-90, 90),
                    faker.Commerce.Department(),
                    faker.Lorem.Paragraph(),
                    new Faker().Person.Email,
                    faker.Person.Phone,
                    faker.Address.FullAddress(),
                    faker.Image.PicsumUrl(),
                    faker.Name.JobTitle(),
                    faker.Person.Email,
                    faker.Person.Phone,
                    faker.Random.Bool(),
                    faker.Address.FullAddress(),
                    faker.Person.DateOfBirth,
                    faker.Name.JobTitle(),
                    faker.Image.PicsumUrl());
                
                await _eventCollaborationRequestRepository.InsertAsync(collaborationRequest);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Seeded event collaboration request failed: {Message}", e.Message);
            }
        }
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Seeded event collaboration requests successfully!");
    }

    private async Task SeedInternalProofAsync()
    {
        if (await _proofRepository.AnyAsync(new ProofByTypeSpecification(ProofType.Internal)))
        {
            _logger.LogInformation("Internal proofs already seeded.");
            return;
        }
        
        var events = await _eventRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();

        foreach (var @event in events)
        {
            try
            {
                var faker = new Faker();
            
                var proof = await _proofDomainService.CreateInternalProofAsync(
                    faker.Lorem.Sentence(),
                    faker.Image.PicsumUrl(),
                    faker.PickRandom(@event.AttendanceInfos).StartAt.AddMinutes(5),
                    faker.PickRandom(students).Id,
                    @event.Id,
                    faker.PickRandom(@event.Roles.Where(x => !x.IsNeedApprove)).Id,
                    @event.EndAt.AddDays(5));
                await _proofRepository.InsertAsync(proof);
            }
            catch (Exception e)
            {
                _logger.LogError("Seeded internal proofs failed: {Message}", e.Message);
            }
        }
        
        await _unitOfWork.CommitAsync();
    }

    private async Task SeedExternalProofAsync()
    {
        if (await _proofRepository.AnyAsync(new ProofByTypeSpecification(ProofType.External)))
        {
            _logger.LogInformation("External proofs already seeded.");
            return;
        }

        var activities = await _eventActivityRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();
        for (var i = 0; i < 30; i++)
        {
            try
            {
                var faker = new Faker();

                var activity = faker.PickRandom(activities);
                var proof = await _proofDomainService.CreateExternalProofAsync(
                    faker.Lorem.Sentence(),
                    faker.Image.PicsumUrl(),
                    DateTime.UtcNow.AddDays(-1.5),
                    faker.PickRandom(students).Id,
                    faker.Lorem.Sentence(),
                    faker.Company.CompanyName(),
                    faker.Address.FullAddress(),
                    faker.Name.JobTitle(),
                    DateTime.UtcNow.AddDays(-2),
                    DateTime.UtcNow.AddDays(-1),
                    faker.Random.Int((int)activity.MinScore, (int)activity.MaxScore),
                    activity.Id);
                await _proofRepository.InsertAsync(proof);
            }
            catch (Exception e)
            {
                _logger.LogError("Seeded external proofs failed: {Message}", e.Message);
            }
        }
        
        await _unitOfWork.CommitAsync();
    }
    
    private Dictionary<dynamic, List<dynamic>> GetEventCategories()
    {
        var eventCategories = new Dictionary<dynamic, List<dynamic>>();
        
        eventCategories.Add(new
        {
            Name = "Lĩnh vực công tác xã hội, tình nguyện, nhân đạo",
            Type = EventCategoryType.Event
        }, new List<dynamic>()
        {
            new
            {
                MinScore = 20,
                MaxScore = 30,
                Name = "Chương trình mùa hè xanh"
            },
            new
            {
                MinScore = 20,
                MaxScore = 30,
                Name = "Chương trình tình nguyện, nhân đạo, xã hội có quy mô và thời gian tham gia từ 03 ngày trở lên"
            },
            new
            {
                MinScore = 10,
                MaxScore = 20,
                Name = "Hoạt động nhân đạo; Hoạt động bảo vệ môi trường, góp phần xây dựng, cải tạo cảnh quan Nhà trường"
            },
            new
            {
                MinScore = 10,
                MaxScore = 20,
                Name = "Hoạt động hỗ trợ công tác khắc phục hậu quả thiên tai, dịch bệnh"
            },
            new
            {
                MinScore = 15,
                MaxScore = 20,
                Name = "Hoạt động đền ơn đáp nghĩa"
            },
            new
            {
                MinScore = 15,
                MaxScore = 20,
                Name = "Hiến máu nhân đạo"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Đóng góp vật chất để ủng hộ cho các hoạt động từ thiện, tình nguyện"
            },
            new
            {
                MinScore = 15,
                MaxScore = 20,
                Name = "Hoạt động khác có tính chất tương tự mà thời gian diễn ra dưới 01 ngày"
            }
        });
        
        eventCategories.Add(new
        {
            Name = "Hoạt động mang tính học thuật",
            Type = EventCategoryType.Individual
        }, new List<dynamic>()
        {
            new
            {
                MinScore = 15,
                MaxScore = 15,
                Name = "Tham gia với tư cách là thành viên BTC hoặc cộng tác viên cho các hoạt động cấp quốc gia, quốc tế được cấp có thẩm quyền xác nhận"
            },
            new
            {
                MinScore = 10,
                MaxScore = 10,
                Name = "Tham dự với tư cách là thành viên BTC hoặc cộng tác viên cho các hoạt động cấp thành phố, cấp ĐHĐN"
            },
            new
            {
                MinScore = 5,
                MaxScore = 7,
                Name = "Tham dự với tư cách là thành viên BTC hoặc cộng tác viên cho các họat động trong trường (từ cấp Khoa trở lên)"
            },
            new
            {
                MinScore = 3,
                MaxScore = 5,
                Name = "Tham dự (cổ vũ) cho các hoạt động các cấp"
            },
            new
            {
                MinScore = 30,
                MaxScore = 30,
                Name = "Có giải cấp Quốc tế"
            },
            new
            {
                MinScore = 25,
                MaxScore = 25,
                Name = "Có giải cấp Quốc gia"
            },
            new
            {
                MinScore = 20,
                MaxScore = 20,
                Name = "Có giải cấp tỉnh, thành phố"
            },
            new
            {
                MinScore = 15,
                MaxScore = 20,
                Name = "Có giải cấp ĐHĐN"
            },
            new
            {
                MinScore = 10,
                MaxScore = 25,
                Name = "Có giải cấp Trường"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Có giải cấp Khoa"
            },
            new
            {
                MinScore = 3,
                MaxScore = 5,
                Name = "Tham gia nhưng không đạt giải tại các cuộc thi (có chứng nhận tham gia của Ban Tổ chức cuộc thi)"
            }
        });
        
        eventCategories.Add(new
        {
            Name = "Tham gia BCH Đoàn, Hội Sinh viên các cấp, Ban cán sự lớp và có đóng góp tích cực",
            Type = EventCategoryType.Individual
        }, new List<dynamic>()
        {
            new
            {
                MinScore = 25,
                MaxScore = 25,
                Name = "Là Ủy viên BCH Đoàn, Ủy viên BCH Hội SV cấp ĐHĐN trở lên, hoàn thành tốt nghiệm vụ và được tập thể ghi nhận"
            },
            new
            {
                MinScore = 20,
                MaxScore = 20,
                Name = "Là Ủy viên BTV Đoàn, Ủy viên BTK Hội SV Trường, hoàn thành tốt nhiệm vụ và được tập thể ghi nhận"
            },
            new
            {
                MinScore = 15,
                MaxScore = 15,
                Name = "Là Ủy viên BCH Đoàn, Ủy viên BCH Hội Sinh viên Trường, Bí thư/Phó Bí thư Liên chi Đoàn, Chi hội trưởng Liên chi hội Chủ nhiệm/Đội trưởng các Câu la bộ/Đội/Nhóm trực thuộc Đoàn Thanh niên - Hội Sinh viên Trường, hoàn thành tốt nhiệm vụ và được tập thể ghi nhận"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Là Ủy viên BCH Liên chi Đoàn - Liên chi Hội, Phó Chủ nhiệm các Câu lạc bộ/Đội/Nhóm trực thuộc Đoàn Thanh niên - Hội Sinh viên Trường, Chủ nhiệm các Câu lạc bộ/Đội/Nhóm trực thuộc Khoa/Liên chi Đoàn; Bí thư Chi Đoàn; Lớp trưởng; hoàn thành tốt nhiệm vụ và được tập thể ghi nhận"
            },
            new
            {
                MinScore = 5,
                MaxScore = 7,
                Name = "Là Phó Bí thư Chi Đoàn, Lớp phó, Phó Chủ nhiệm các Câu lạc bộ/Đội/Nhóm trực thuộc Khoa/Liên chi Đoàn; hoàn thành tốt nhiệm vụ và được tập thể ghi nhận"
            },
            new
            {
                MinScore = 3,
                MaxScore = 5,
                Name = "Là Ủy viên BCH Chi Đoàn - Chi Hội, thành viên Ban Chủ nhiệm/Ban Điều hành các Câu lạc bộ/Đội/Nhóm trực thuộc Đoàn Thanh niên/Hội SV, Khoa, Liên Chi Đoàn, hoàn thành tốt nhiệm vụ và được tập thể ghi nhận"
            }
        });
        
        eventCategories.Add(new
        {
            Name = "Hỗ trợ, cộng tác viên thường xuyên cho Nhà trường và Đại học Đà Nẵng",
            Type = EventCategoryType.Individual
        }, new List<dynamic>()
        {
            new
            {
                MinScore = 10,
                MaxScore = 20,
                Name = "Tham gia hỗ trợ, cộng tác viên thường xuyên cho Nhà trường và Đại học Đà Nẵng"
            }
        });
        
        eventCategories.Add(new
        {
            Name = "Các hoạt động khác",
            Type = EventCategoryType.Event
        }, new List<dynamic>()
        {
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Gặp mặt, giao lưu, trao đổi với doanh nghiệp, cựu sinh viên"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Tọa đàm về giáo dục giới tính, bình đẳng giới"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Tọa đàm về văn hóa ứng xử"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Tọa đàm về phòng chống bạo hành và xâm phạm trẻ em, pháp luật, an toàn giao thông, bảo vệ biển đảo"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Tọa đàm về pháp luật, an toàn giao thông, bảo vệ biển đảo"
            },
            new
            {
                MinScore = 5,
                MaxScore = 5,
                Name = "Buổi gặp mặt, đối thoại với Hiệu trưởng và lãnh đao Nhà trường"
            },
            new
            {
                MinScore = 5,
                MaxScore = 20,
                Name = "Lĩnh vực đào tạo và đảm bảo chất lượng giáo dục đào tạo"
            },
            new
            {
                MinScore = 20,
                MaxScore = 30,
                Name = "Lĩnh vực nghiên cứu khoa học và chuyển giao công nghệ"
            },
            new
            {
                MinScore = 5,
                MaxScore = 20,
                Name = "Lĩnh vực tư vấn"
            },
            new
            {
                MinScore = 5,
                MaxScore = 10,
                Name = "Hoạt động công tác sinh viên thường niên"
            }
        });
        return eventCategories;
    }
}