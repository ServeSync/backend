using AutoMapper;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement;

public class StudentManagementMapperProfile : Profile
{
    public StudentManagementMapperProfile()
    {
        CreateMap<EducationProgram, EducationProgramDto>();
        CreateMap<Faculty, FacultyDto>();
        CreateMap<HomeRoom, HomeRoomDto>();
    }
}