using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.HomeRooms.Queries;

public class GetAllHomeRoomQueryHandler : IQueryHandler<GetAllHomeRoomQuery, IEnumerable<HomeRoomDto>>
{
    private readonly IBasicReadOnlyRepository<HomeRoom, Guid> _homeRoomRepository;
    private readonly IMapper _mapper;

    public GetAllHomeRoomQueryHandler(
        IBasicReadOnlyRepository<HomeRoom, Guid> homeRoomRepository,
        IMapper mapper)
    {
        _homeRoomRepository = homeRoomRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<HomeRoomDto>> Handle(GetAllHomeRoomQuery request, CancellationToken cancellationToken)
    {
        var homeRooms = await _homeRoomRepository.FilterAsync(new FilterHomeRoomSpecification(request.FacultyId));
        return _mapper.Map<IEnumerable<HomeRoomDto>>(homeRooms);
    }
}