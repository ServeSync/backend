using AutoMapper;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement;

public class StudentManagementMapperProfile : Profile
{
    public StudentManagementMapperProfile()
    {
        CreateMap<EducationProgram, EducationProgramDto>();
        CreateMap<EducationProgram, StudentEducationProgramDto>();
        CreateMap<EducationProgramDto, StudentEducationProgramDto>();
        
        CreateMap<Faculty, FacultyDto>();
        
        CreateMap<HomeRoom, HomeRoomDto>();

        CreateMap<Student, BasicStudentDto>();
        CreateMap<Student, FlatStudentDto>()
            .ForMember(dest => dest.FacultyId,
                opt => opt.MapFrom(src => src.HomeRoom!.FacultyId));

        CreateMap<Student, StudentDetailDto>()
            .ForMember(dest => dest.HomeRoom,
                opt => opt.MapFrom(src => new HomeRoomDto()
                    { Id = src.HomeRoomId, Name = src.HomeRoom!.Name, FacultyId = src.HomeRoom.FacultyId }))
            .ForMember(dest => dest.EducationProgram,
                opt => opt.MapFrom(src => new EducationProgramDto()
                    { Id = src.EducationProgramId, Name = src.EducationProgram!.Name }))
            .ForMember(dest => dest.Faculty,
                opt => opt.MapFrom(src => new FacultyDto()
                    { Id = src.HomeRoom!.FacultyId, Name = src.HomeRoom.Faculty!.Name }))
            .ForMember(dest => dest.EducationProgram, opt => opt.MapFrom(src => new EducationProgramDto()
                    {
                        Id = src.EducationProgramId, Name = src.EducationProgram!.Name,
                        RequiredActivityScore = src.EducationProgram.RequiredActivityScore,
                        RequiredCredit = src.EducationProgram.RequiredCredit
                    }))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.EventRegisters.Sum(x => x.StudentEventAttendance != null ? x.EventRole!.Score : 0)));

        CreateMap<Student, SimpleStudentDto>();

        CreateMap<Proof, ProofDto>()
            .ForMember(dest => dest.EventName,
                opt => opt.MapFrom(src =>
                    src.InternalProof != null ? src.InternalProof!.Event!.Name : src.ExternalProof!.EventName))
            .ForMember(dest => dest.OrganizationName,
                opt => opt.MapFrom(src =>
                    src.InternalProof != null
                        ? src.InternalProof!.Event!.RepresentativeOrganization!.Organization!.Name
                        : src.ExternalProof!.OrganizationName))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src =>
                    src.InternalProof != null
                        ? src.InternalProof!.Event!.Address.FullAddress
                        : src.ExternalProof!.Address))
            .ForMember(dest => dest.StartAt,
                opt => opt.MapFrom(src =>
                    src.InternalProof != null ? src.InternalProof!.Event!.StartAt : src.ExternalProof!.StartAt))
            .ForMember(dest => dest.EndAt,
                opt => opt.MapFrom(src =>
                    src.InternalProof != null ? src.InternalProof!.Event!.EndAt : src.ExternalProof!.EndAt));

    }
}