using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetRolesForUserQueryHandler : IQueryHandler<GetRolesForUserQuery, List<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    
    public GetRolesForUserQueryHandler(IRoleRepository roleRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    public async Task<List<RoleDto>> Handle(GetRolesForUserQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.FindByUserAsync(request.UserId, request.TenantId);
        return _mapper.Map<List<RoleDto>>(roles);
    }
}