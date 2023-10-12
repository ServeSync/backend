using Bogus;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

namespace ServeSync.Application.Seeders;

public class EventManagementDataSeeder : IDataSeeder
{
    private readonly IEventCategoryDomainService _eventCategoryDomainService;
    private readonly IEventOrganizationDomainService _eventOrganizationDomainService;
    
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    
    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public EventManagementDataSeeder(
        IEventCategoryDomainService eventCategoryDomainService,
        IEventOrganizationDomainService eventOrganizationDomainService,
        IEventCategoryRepository eventCategoryRepository,
        IEventOrganizationRepository eventOrganizationRepository,
        ILogger<StudentManagementDataSeeder> logger, 
        IUnitOfWork unitOfWork)
    {
        _eventCategoryDomainService = eventCategoryDomainService;
        _eventOrganizationDomainService = eventOrganizationDomainService;
        _eventCategoryRepository = eventCategoryRepository;
        _eventOrganizationRepository = eventOrganizationRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        await SeedEventCategoriesAsync();
        await SeedEventOrganizationsAsync();
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
            for (var i = 0; i < 10; i++)
            {
                var faker = new Faker();
                var eventOrganization = await _eventOrganizationDomainService.CreateAsync(
                    faker.Company.CompanyName(),
                    faker.Internet.Email(),
                    faker.Phone.PhoneNumber(),
                    faker.Image.PicsumUrl(),
                    faker.Lorem.Sentence(),
                    faker.Address.FullAddress());

                for (var j = 0; j < 10; j++)
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
}