using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Specifications;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

public class GetAllPermissionQueryHandler : IQueryHandler<GetAllPermissionQuery, IEnumerable<PermissionDto>>
{
    private readonly IBasicReadOnlyRepository<ApplicationPermission, Guid> _permissionRepository;
    private readonly IMapper _mapper;

    public GetAllPermissionQueryHandler(
        IBasicReadOnlyRepository<ApplicationPermission, Guid> permissionRepository,
        IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.FilterAsync(new PermissionByNameSpecification(request.Name));
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}