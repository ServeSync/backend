using AutoMapper;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Permissions.Mapper;

public class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<ApplicationPermission, PermissionDto>();
    }
}