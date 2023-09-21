using AutoMapper;
using ServeSync.Application.Identity.Dtos;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<ApplicationUser, IdentityUserDto>();
    }
}