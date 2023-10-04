using AutoMapper;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

namespace ServeSync.Application.Caching;

public class FacultyCachingManager : CachingManager<Faculty>, IFacultyCachingManager
{
    private readonly ICachingService _cacheService;
    private readonly IBasicReadOnlyRepository<Faculty, Guid> _facultyRepository;
    private readonly ILogger<EducationCachingManager> _logger;
    private readonly IMapper _mapper;
    
    public FacultyCachingManager(
        ICachingService cacheService,
        IBasicReadOnlyRepository<Faculty, Guid> facultyRepository,
        ILogger<EducationCachingManager> logger,
        IMapper mapper)
    {
        _cacheService = cacheService;
        _facultyRepository = facultyRepository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task SetAsync(IList<FacultyDto> faculties)
    {
        await _cacheService.SetRecordAsync(ResourceName, faculties, TimeSpan.FromDays(DefaultCacheDurationInDay));
    }

    public Task<IList<FacultyDto>?> GetAsync()
    {
        return _cacheService.GetRecordAsync<IList<FacultyDto>?>(ResourceName);
    }

    public async Task<IList<FacultyDto>> GetOrAddAsync()
    {
        var faculties = await GetAsync();
        if (faculties == null)
        {
            _logger.LogInformation("Cache missed faculties!");
            
            faculties = _mapper.Map<IList<FacultyDto>>(await _facultyRepository.FindAllAsync());
            await SetAsync(faculties);
        }

        return faculties;
    }
}