using AutoMapper;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement;

public class EventManagementMapperProfile : Profile
{
    public EventManagementMapperProfile()
    {
        CreateMap<EventActivity, EventActivityDto>();

        CreateMap<EventOrganization, EventOrganizationDto>();

        CreateMap<EventOrganizationContact, EventOrganizationContactDto>();
    }
}