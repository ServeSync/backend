using AutoMapper;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;

namespace ServeSync.Application.ReadModels.Mappers;

public class EventReadModelMapper : Profile
{
    public EventReadModelMapper()
    {
        CreateMap<EventReadModel, EventDetailDto>();
        CreateMap<EventRoleReadModel, EventRoleDto>();
        CreateMap<EventRegistrationInfoReadModel, EventRegistrationDto>();
        CreateMap<RegisteredStudentInEventRoleReadModel, RegisteredStudentInEventRoleDto>();
        CreateMap<RegisteredStudentInEventReadModel, RegisteredStudentInEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.StudentId));
        CreateMap<AttendanceStudentInEventRoleReadModel, AttendanceStudentInEventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.StudentId));
        CreateMap<EventAttendanceInfoReadModel, EventAttendanceInfoDto>();
        CreateMap<EventActivityReadModel, BasicEventActivityDto>();
        CreateMap<BasicEventOrganizationInEventReadModel, BasicOrganizationInEventDto>();
        CreateMap<EventOrganizationInEventReadModel, OrganizationInEventDto>();
        CreateMap<EventOrganizationRepresentativeInEventReadModel, BasicRepresentativeInEventDto>();
        CreateMap<EventAddressReadModel, EventAddressDto>();
    }
}