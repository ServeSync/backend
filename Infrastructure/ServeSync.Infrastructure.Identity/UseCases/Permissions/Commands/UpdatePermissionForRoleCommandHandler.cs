using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Commands;

public class UpdatePermissionForRoleCommandHandler : ICommandHandler<UpdatePermissionForRoleCommand, IEnumerable<PermissionDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdatePermissionForRoleCommandHandler(
        IRoleRepository roleRepository, 
        IPermissionRepository permissionRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PermissionDto>> Handle(UpdatePermissionForRoleCommand request, CancellationToken cancellationToken)
    {
        var permissionIds = request.PermissionIds.Distinct();
        var role = await _roleRepository.FindByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new RoleNotFoundException(request.RoleId);
        }

        var permissions = await _permissionRepository.FilterAsync(new PermissionInSpecification(permissionIds));
        if (permissions.Count != permissionIds.Count())
        {
            var notFoundPermissionId = permissionIds.FirstOrDefault(x => permissions.Any(y => y.Id == x));
            throw new PermissionNotFoundException(notFoundPermissionId);
        }
        
        role.ClearPermission();
        UpdatePermission(role, permissionIds);
        
        await _unitOfWork.CommitAsync();
        
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }

    private void UpdatePermission(ApplicationRole role, IEnumerable<Guid> permissionIds)
    {
        foreach (var permissionId in permissionIds)
        {
            role.GrantPermission(permissionId);    
        }
        
        _roleRepository.Update(role);
    }
}