using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Specifications;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetAllUsersQueryHandler: IQueryHandler<GetAllUsersQuery, PagedResultDto<UserBasicInfoDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<UserBasicInfoDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);
        var users = await _userRepository.GetPagedListAsync(specification);
        var total = await _userRepository.GetCountAsync(specification);
        return new PagedResultDto<UserBasicInfoDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<UserBasicInfoDto>>(users));
    }
    
    private IPagingAndSortingSpecification<ApplicationUser, string> GetSpecification(GetAllUsersQuery request)
    {
        var specification = new FilterApplicationUserSpecification(request.Page, request.Size, request.Sorting, request.Search);
        return specification;
    }
}