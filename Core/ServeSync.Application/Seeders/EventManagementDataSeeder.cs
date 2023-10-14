using Bogus;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.Seeders;

public class EventManagementDataSeeder : IDataSeeder
{
    private readonly IEventCategoryDomainService _eventCategoryDomainService;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    private readonly IEventDomainService _eventDomainService;
    
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository; 
    
    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public EventManagementDataSeeder(
        IEventCategoryDomainService eventCategoryDomainService,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IEventDomainService eventDomainService,
        IEventCategoryRepository eventCategoryRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        IEventRepository eventRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        ILogger<StudentManagementDataSeeder> logger, 
        IUnitOfWork unitOfWork)
    {
        _eventCategoryDomainService = eventCategoryDomainService;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _eventDomainService = eventDomainService;
        _eventCategoryRepository = eventCategoryRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventRepository = eventRepository;
        _eventActivityRepository = eventActivityRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        await SeedEventCategoriesAsync();
        await SeedEventOrganizationsAsync();
        await SeedEventsAsync();
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
        
        for (var i = 0; i < 50; i++)
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
                    faker.Date.Between(DateTime.Now, DateTime.Now.AddDays(10)),
                    faker.Date.Between(DateTime.Now.AddDays(10), DateTime.Now.AddDays(20)),
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
                    faker.Date.Between(DateTime.Now, DateTime.Now.AddDays(10)),
                    faker.Date.Between(DateTime.Now.AddDays(10), DateTime.Now.AddDays(20)));

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

                await _unitOfWork.CommitAsync();

                _eventDomainService.SetRepresentativeOrganization(
                    @event,
                    faker.PickRandom(@event.Organizations).OrganizationId);

                await _unitOfWork.CommitTransactionAsync(true);

                _logger.LogInformation("Seeded event organizations successfully!");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Seeded event organizations failed: {Message}", e.Message);
            }
        }
    }
}