using AutoMapper;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Application.Caching;

public class HomeRoomCachingManager : CachingManager<HomeRoom>, IHomeRoomCachingManager
{
    private readonly ICachingService _cacheService;
    private readonly IBasicReadOnlyRepository<HomeRoom, Guid> _homeRoomRepository;
    private readonly ILogger<EducationCachingManager> _logger;
    private readonly IMapper _mapper;
    
    public HomeRoomCachingManager(
        ICachingService cacheService,
        IBasicReadOnlyRepository<HomeRoom, Guid> homeRoomRepository,
        ILogger<EducationCachingManager> logger,
        IMapper mapper)
    {
        _cacheService = cacheService;
        _homeRoomRepository = homeRoomRepository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task SetAsync(IList<HomeRoomDto> homeRooms)
    {
        await _cacheService.SetRecordAsync(ResourceName, homeRooms, TimeSpan.FromDays(DefaultCacheDurationInDay));
    }

    public Task<IList<HomeRoomDto>?> GetAsync()
    {
        return _cacheService.GetRecordAsync<IList<HomeRoomDto>?>(ResourceName);
    }

    public async Task<IList<HomeRoomDto>> GetOrAddAsync()
    {
        var homeRooms = await GetAsync();
        if (homeRooms == null)
        {
            _logger.LogInformation("Cache missed home rooms!");
            
            homeRooms = _mapper.Map<IList<HomeRoomDto>>(await _homeRoomRepository.FindAllAsync());
            await SetAsync(homeRooms);
        }

        return homeRooms;
    }

    public async Task<IList<HomeRoomDto>> GetOrAddForFacultyAsync(Guid facultyId)
    {
        return (await GetOrAddAsync()).Where(x => x.FacultyId == facultyId).ToList();
    }
}