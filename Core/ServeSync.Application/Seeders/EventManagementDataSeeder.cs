using Bogus;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;

namespace ServeSync.Application.Seeders;

public class EventManagementDataSeeder : IDataSeeder
{
    private readonly IEventCategoryDomainService _eventCategoryDomainService;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly ILogger<StudentManagementDataSeeder> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public EventManagementDataSeeder(
        IEventCategoryDomainService eventCategoryDomainService,
        IEventCategoryRepository eventCategoryRepository,
        ILogger<StudentManagementDataSeeder> logger, 
        IUnitOfWork unitOfWork)
    {
        _eventCategoryDomainService = eventCategoryDomainService;
        _eventCategoryRepository = eventCategoryRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        await SeedEventCategoriesAsync();
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
}