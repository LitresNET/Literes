using AutoMapper;
using backend.Dto.Requests;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Configurations.Mapping;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegistrationDto, User>().AfterMap((dto, user) =>
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, dto.Password));
    }
}