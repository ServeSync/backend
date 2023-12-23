using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Tenants.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDetailDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;
    
    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
        {
            throw new UserNotFoundException(request.Id);
        }
        
        var tenants = await _tenantRepository.FindByUserAsync(user.Id);
        
        var userInfo = new UserDetailDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Tenants = _mapper.Map<IEnumerable<TenantDto>>(tenants)
        };
        
        return userInfo;
    }
}