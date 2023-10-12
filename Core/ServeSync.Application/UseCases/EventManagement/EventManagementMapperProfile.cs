using AutoMapper;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement;

public class EventManagementMapperProfile : Profile
{
    public EventManagementMapperProfile()
    {
        CreateMap<EventActivity, EventActivityDto>();
    }
}