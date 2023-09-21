using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionForRoleQueryHandler : IQueryHandler<GetAllPermissionForRoleQuery, IEnumerable<PermissionDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IBasicReadOnlyRepository<ApplicationPermission, Guid> _permissionRepository;
    private readonly IMapper _mapper;

    public GetAllPermissionForRoleQueryHandler(
        IRoleRepository roleRepository,
        IBasicReadOnlyRepository<ApplicationPermission, Guid> permissionRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionForRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new RoleNotFoundException(request.RoleId);
        }

        var specification = new PermissionInSpecification(role.Permissions.Select(x => x.PermissionId)).And(
            new PermissionByNameSpecification(request.Name));
        
        var permissions = await _permissionRepository.FilterAsync(specification);

        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}