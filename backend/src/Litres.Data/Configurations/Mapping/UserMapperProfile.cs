using AutoMapper;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegistrationDto, User>()
            .ForMember("UserName", opt => opt.MapFrom(dto => dto.Email))
            .ForMember("PasswordHash", opt => opt.MapFrom(dto => dto.Password));

        CreateMap<UserSettingsDto, User>();
        CreateMap<User, UserSettingsDto>();
    }
}