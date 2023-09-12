using AutoMapper;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Mapper;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<ApplicationRole, RoleDto>();
    }
}