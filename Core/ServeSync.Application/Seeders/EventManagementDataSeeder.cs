using Bogus;
using Microsoft.Extensions.Logging;
using Polly;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.SeedWorks.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.Seeders;

public class EventManagementDataSeeder : IDataSeeder
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IEventCategoryDomainService _eventCategoryDomainService;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IEventDomainService _eventDomainService;
    
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IBasicReadOnlyRepository<StudentEventRegister, Guid> _studentEventRegisterRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    private readonly IBasicReadOnlyRepository<StudentEventAttendance, Guid> _studentAttendanceRepository;
    
    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public EventManagementDataSeeder(
        IStudentDomainService studentDomainService,
        IEventCategoryDomainService eventCategoryDomainService,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IEventDomainService eventDomainService,
        IEventCategoryRepository eventCategoryRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        IEventRepository eventRepository,
        IStudentRepository studentRepository,
        IBasicReadOnlyRepository<StudentEventRegister, Guid> studentEventRegisterRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        IBasicReadOnlyRepository<StudentEventAttendance, Guid> studentAttendanceRepository,
        ILogger<StudentManagementDataSeeder> logger, 
        IUnitOfWork unitOfWork)
    {
        _studentDomainService = studentDomainService;
        _eventCategoryDomainService = eventCategoryDomainService;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _eventDomainService = eventDomainService;
        _eventCategoryRepository = eventCategoryRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventRepository = eventRepository;
        _studentRepository = studentRepository;
        _studentEventRegisterRepository = studentEventRegisterRepository;
        _eventActivityRepository = eventActivityRepository;
        _studentAttendanceRepository = studentAttendanceRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        await SeedEventCategoriesAsync();
        await SeedEventOrganizationsAsync();
        await SeedEventsAsync();
        await SeedRegisterEventAsync();
        await SeedAttendanceEventsForStudentAsync();
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
            for (var i = 0; i < 10; i++)
            {
                var faker = new Faker();
                var eventCategory = await _eventCategoryDomainService.CreateAsync(faker.Company.CompanyName());

                for (var j = 0; j < 10; j++)
                {
                    var minScore = faker.Random.Double(0, 20);
                    _eventCategoryDomainService.AddActivity(eventCategory, faker.Commerce.ProductName(), minScore, faker.Random.Double(minScore, 20));
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
            for (var i = 0; i < 50; i++)
            {
                var faker = new Faker();
                var eventOrganization = await _eventOrganizationDomainService.CreateAsync(
                    faker.Company.CompanyName(),
                    faker.Internet.Email(),
                    faker.Phone.PhoneNumber(),
                    faker.Image.PicsumUrl(),
                    faker.Lorem.Sentence(),
                    faker.Address.FullAddress());

                for (var j = 0; j < 50; j++)
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
                        faker.Commerce.Department());
                }
            }
        
            await _unitOfWork.CommitAsync();
        
            _logger.LogInformation("Seeded event organizations successfully!");
        }
        catch (Exception e)
        {
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
        var eventActivities = await _eventActivityRepository.FindAllAsync();
       
        for (var i = 0; i < 100; i++)
        {
            try
            {
                var faker = new Faker();
                await _unitOfWork.BeginTransactionAsync();

                var @event = await _eventDomainService.CreateAsync(
                    faker.Person.FullName,
                    faker.Lorem.Sentence(),
                    faker.Lorem.Paragraph(),
                    faker.Image.PicsumUrl(),
                    faker.PickRandom<EventType>(),
                    faker.Date.Between(DateTime.Now.AddHours(1), DateTime.Now.AddDays(2)),
                    faker.Date.Between(DateTime.Now.AddDays(0), DateTime.Now.AddDays(2)),
                    faker.PickRandom(eventActivities).Id,
                    faker.Address.FullAddress(),
                    faker.Random.Double(-180, 180),
                    faker.Random.Double(-90, 90));

                for (var j = 0; j < 3; j++)
                {
                    faker = new Faker();
                    _eventDomainService.AddRole(
                        @event,
                        faker.Name.JobTitle(),
                        faker.Lorem.Sentence(),
                        faker.Random.Bool(),
                        faker.Random.Int(1, 10),
                        faker.Random.Int(1, 10));
                }

                _eventDomainService.AddAttendanceInfo(
                    @event,
                    faker.Date.Between(DateTime.Now.AddMinutes(30), DateTime.Now.AddDays(1)),
                    faker.Date.Between(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)));

                _eventDomainService.AddRegistrationInfo(
                    @event,
                    faker.Date.Between(DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(45)),
                    faker.Date.Between(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3)),
                    DateTime.Now);

                var organization = faker.PickRandom(eventOrganizations);
                
                _eventDomainService.AddOrganization(
                    @event,
                    organization,
                    faker.Name.JobTitle());

                _eventDomainService.AddRepresentative(
                    @event,
                    organization,
                    faker.PickRandom(organization.Contacts),
                    faker.Name.JobTitle());
                
                @event.Approve();

                await _eventRepository.InsertAsync(@event);
                await _unitOfWork.CommitAsync();

                _eventDomainService.SetRepresentativeOrganization(
                    @event,
                    faker.PickRandom(@event.Organizations).OrganizationId);

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
        
        var policy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.LogWarning(ex, "Couldn't seed student event registration table after {TimeOut}s", $"{time.TotalSeconds:n1}");
                }
            );
        
        policy.Execute(() =>
        {
            if (!_eventRepository.AnyAsync().Result)
            {
                throw new Exception("Events not seeded yet.");
            }
        
            if (!_studentRepository.AnyAsync().Result)
            {
                throw new Exception("Students not seeded yet.");
            }
        });
        
        var events = await _eventRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();
        foreach (var @event in events)
        {
            try
            {
                var faker = new Faker();
                for (var i = 0; i < 10; i++)
                {
                    await _studentDomainService.RegisterEventAsync(
                        faker.PickRandom(students),
                        faker.PickRandom(@event.Roles).Id,
                        faker.Lorem.Sentence(),
                        faker.PickRandom(@event.RegistrationInfos).StartAt.AddMinutes(1));    
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Seeded student event registrations failed: {Message}", e.Message);
            }
        }
        
        _logger.LogInformation("Seeded student event registrations success!");
    }

    private async Task SeedAttendanceEventsForStudentAsync()
    {
        if (await _studentAttendanceRepository.AnyAsync())
        {
            _logger.LogInformation("Student event attendances already seeded.");
            return;
        }
        
        var policy = Policy.Handle<Exception>()
            .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.LogWarning(ex, "Couldn't seed student event registration table after {TimeOut}s", $"{time.TotalSeconds:n1}");
                }
            );
        
        policy.Execute(() =>
        {
            if (!_eventRepository.AnyAsync().Result)
            {
                throw new Exception("Events not seeded yet.");
            }
        
            if (!_studentRepository.AnyAsync().Result)
            {
                throw new Exception("Students not seeded yet.");
            }

            if (!_studentEventRegisterRepository.AnyAsync().Result)
            {
                throw new Exception("Student event registrations not seeded yet.");
            }
        });
        
        var events = await _eventRepository.FindAllAsync();
        var students = await _studentRepository.FindAllAsync();
        foreach (var @event in events)
        {
            var faker = new Faker();
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var student = faker.PickRandom(students);
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
}