﻿using AutoMapper;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;
using TenantDto = ServeSync.Infrastructure.Identity.UseCases.Tenants.Dtos.TenantDto;

namespace ServeSync.Infrastructure.Identity.UseCases.Tenants.Mappers;

public class TenantMapper : Profile
{
    public TenantMapper()
    {
        CreateMap<Tenant, TenantDto>();
        CreateMap<Tenant, ServeSync.Application.Identity.Dtos.TenantDto>();
    }
}