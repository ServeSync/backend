using AutoMapper;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.HomeRooms.Queries;

public class GetAllHomeRoomQueryHandler : IQueryHandler<GetAllHomeRoomQuery, IEnumerable<HomeRoomDto>>
{
    private readonly IHomeRoomCachingManager _homeRoomCachingManager;

    public GetAllHomeRoomQueryHandler(IHomeRoomCachingManager homeRoomCachingManager)
    {
        _homeRoomCachingManager = homeRoomCachingManager;
    }
    
    public async Task<IEnumerable<HomeRoomDto>> Handle(GetAllHomeRoomQuery request, CancellationToken cancellationToken)
    {
        if (request.FacultyId.HasValue)
        {
            return await _homeRoomCachingManager.GetOrAddForFacultyAsync(request.FacultyId.Value);
        }
        
        return await _homeRoomCachingManager.GetOrAddAsync();
    }
}