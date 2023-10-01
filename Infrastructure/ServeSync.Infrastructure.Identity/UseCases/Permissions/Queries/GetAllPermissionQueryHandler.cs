using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionQueryHandler : IQueryHandler<GetAllPermissionQuery, IEnumerable<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;

    public GetAllPermissionQueryHandler(
        IPermissionRepository permissionRepository,
        IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.FilterAsync(request.Name);
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}