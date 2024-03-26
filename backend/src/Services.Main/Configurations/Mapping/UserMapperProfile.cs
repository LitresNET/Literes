using AutoMapper;
using backend.Dto.Requests;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Configurations.Mapping;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegistrationDto, User>()
            .ForMember("UserName", opt => opt.MapFrom(dto => dto.Email))
            .ForMember("PasswordHash", opt => opt.MapFrom(dto => dto.Password));
    }
}