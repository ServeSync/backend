using AutoMapper;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

namespace ServeSync.Application.Caching;

public class EducationCachingManager : CachingManager<EducationProgram>, IEducationCachingManager
{
    private readonly ICachingService _cacheService;
    private readonly IBasicReadOnlyRepository<EducationProgram, Guid> _educationProgramRepository;
    private readonly ILogger<EducationCachingManager> _logger;
    private readonly IMapper _mapper;
    
    public EducationCachingManager(
        ICachingService cacheService,
        IBasicReadOnlyRepository<EducationProgram, Guid> educationProgramRepository,
        ILogger<EducationCachingManager> logger,
        IMapper mapper)
    {
        _cacheService = cacheService;
        _educationProgramRepository = educationProgramRepository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task SetAsync(IList<EducationProgramDto> educationProgram)
    {
        await _cacheService.SetRecordAsync(ResourceName, educationProgram, TimeSpan.FromDays(DefaultCacheDurationInDay));
    }

    public Task<IList<EducationProgramDto>?> GetAsync()
    {
        return _cacheService.GetRecordAsync<IList<EducationProgramDto>?>(ResourceName);
    }

    public async Task<IList<EducationProgramDto>> GetOrAddAsync()
    {
        var educationPrograms = await GetAsync();
        if (educationPrograms == null)
        {
            _logger.LogInformation("Cache missed education programs!");
            
            educationPrograms = _mapper.Map<IList<EducationProgramDto>>(await _educationProgramRepository.FindAllAsync());
            await SetAsync(educationPrograms);
        }

        return educationPrograms;
    }
}