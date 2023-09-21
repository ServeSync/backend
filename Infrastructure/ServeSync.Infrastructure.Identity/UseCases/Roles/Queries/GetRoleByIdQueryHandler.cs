using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IBasicReadOnlyRepository<ApplicationRole, string> _roleRepository;
    private readonly IMapper _mapper;
    public GetRoleByIdQueryHandler(
        IBasicReadOnlyRepository<ApplicationRole, string> roleRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindByIdAsync(request.Id);
        if (role == null)
        {
            throw new RoleNotFoundException(request.Id);
        }
        
        return _mapper.Map<RoleDto>(role);
    }
}