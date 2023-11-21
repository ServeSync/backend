using AutoMapper;
using ServeSync.Application.Identity.Dtos;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.UseCases.Tenants.Mappers;

public class TenantMapper : Profile
{
    public TenantMapper()
    {
        CreateMap<Tenant, TenantDto>();
    }
}