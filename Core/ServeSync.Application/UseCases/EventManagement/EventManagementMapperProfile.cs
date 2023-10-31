﻿using AutoMapper;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.ValueObjects;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;

namespace ServeSync.Application.UseCases.EventManagement;

public class EventManagementMapperProfile : Profile
{
    public EventManagementMapperProfile()
    {
        CreateMap<EventActivity, BasicEventActivityDto>();
        CreateMap<EventActivity, EventActivityDto>();

        CreateMap<EventOrganization, EventOrganizationDto>();

        CreateMap<EventCategory, EventCategoryDto>();

        CreateMap<EventOrganizationContact, EventOrganizationContactDto>();

        CreateMap<EventAddress, EventAddressDto>();
        
        CreateMap<Event, BasicEventDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.GetStatus(DateTime.Now)))
            .ForMember(dest => dest.CalculatedStatus, opt => opt.MapFrom(src => src.GetCurrentStatus(DateTime.Now)));
        CreateMap<Event, FlatEventDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.GetStatus(DateTime.Now)))
            .ForMember(dest => dest.CalculatedStatus, opt => opt.MapFrom(src => src.GetCurrentStatus(DateTime.Now)))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Roles.Sum(x => x.Quantity)))
            .ForMember(dest => dest.Registered, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 0));
        CreateMap<Event, EventDetailDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.GetStatus(DateTime.Now)))
            .ForMember(dest => dest.CalculatedStatus, opt => opt.MapFrom(src => src.GetCurrentStatus(DateTime.Now)))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Roles.Sum(x => x.Quantity)))
            .ForMember(dest => dest.Registered, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 0));
        
        CreateMap<OrganizationInEvent, BasicOrganizationInEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrganizationId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Organization!.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Organization!.ImageUrl));
        CreateMap<OrganizationInEvent, OrganizationInEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrganizationId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Organization!.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Organization!.ImageUrl));
        
        CreateMap<OrganizationRepInEvent, BasicRepresentativeInEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrganizationRepId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrganizationRep!.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.OrganizationRep!.ImageUrl))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.OrganizationRep!.Position));
        
        CreateMap<EventCollaborationRequest, EventCollaborationRequestCreateDto>();

        CreateMap<EventOrganization, EventOrganizationInfoDto>();

        CreateMap<EventOrganizationContact, EventOrganizationContactInfoDto>();
        
        CreateMap<EventAttendanceInfo, EventAttendanceInfoDto>();

        CreateMap<EventRole, EventRoleDto>();

        CreateMap<EventRegistrationInfo, EventRegistrationDto>();

        CreateMap<EventOrganizationInfo, EventOrganizationInfoDto>();
        
        CreateMap<EventCollaborationRequest, EventCollaborationRequestDto>();
    }
}