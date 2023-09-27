using AutoMapper;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement;

public class StudentManagementMapperProfile : Profile
{
    public StudentManagementMapperProfile()
    {
        CreateMap<EducationProgram, EducationProgramDto>();
        
        CreateMap<Faculty, FacultyDto>();
        
        CreateMap<HomeRoom, HomeRoomDto>();

        CreateMap<Student, BasicStudentDto>();
        CreateMap<Student, FlatStudentDto>()
            .ForMember(dest => dest.FacultyId,
                opt => opt.MapFrom(src => src.HomeRoom.FacultyId));
        
        CreateMap<Student, StudentDetailDto>()
            .ForMember(dest => dest.HomeRoom,
                opt => opt.MapFrom(src => new HomeRoomDto()
                    { Id = src.HomeRoomId, Name = src.HomeRoom.Name, FacultyId = src.HomeRoom.FacultyId }))
            .ForMember(dest => dest.EducationProgram,
                opt => opt.MapFrom(src => new EducationProgramDto()
                    { Id = src.EducationProgramId, Name = src.EducationProgram.Name }))
            .ForMember(dest => dest.Faculty,
                opt => opt.MapFrom(src => new FacultyDto() 
                    { Id = src.HomeRoom.FacultyId, Name = src.HomeRoom.Faculty.Name }));
    }
}