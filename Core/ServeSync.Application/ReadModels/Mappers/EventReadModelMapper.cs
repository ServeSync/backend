using AutoMapper;
using ServeSync.Application.Common.Dtos;
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
        CreateMap<RegisteredStudentInEventReadModel, RegisteredStudentInEventDto>();
        CreateMap<AttendanceStudentInEventRoleReadModel, AttendanceStudentInEventDto>();
        CreateMap<EventAttendanceInfoReadModel, EventAttendanceInfoDto>();
        CreateMap<EventActivityReadModel, BasicEventActivityDto>();
        CreateMap<EventActivityReadModel, EventActivityDto>();
        CreateMap<EventActivityReadModel, EventActivityDetailDto>();
        CreateMap<BasicEventOrganizationInEventReadModel, BasicOrganizationInEventDto>();
        CreateMap<EventOrganizationInEventReadModel, OrganizationInEventDto>();
        CreateMap<EventOrganizationRepresentativeInEventReadModel, BasicRepresentativeInEventDto>();
        CreateMap<EventAddressReadModel, EventAddressDto>();
        CreateMap<EventReadModel, StudentAttendanceEventDto>();
        CreateMap<EventReadModel, StudentRegisteredEventDto>();
        CreateMap<AttendanceStudentInEventRoleReadModel, StudentAttendanceEventDto>();
        CreateMap<AuditReadModel, AuditDto>();
    }
}