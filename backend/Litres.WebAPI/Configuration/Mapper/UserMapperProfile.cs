using AutoMapper;
using Litres.Application.Commands.SignUp;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;

namespace Litres.WebAPI.Configuration.Mapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserRegistrationDto, User>()
            .ForMember("UserName", opt => opt.MapFrom(dto => dto.Email))
            .ForMember("PasswordHash", opt => opt.MapFrom(dto => dto.Password));
        CreateMap<SignUpUserCommand, User>()
            .ForMember("UserName", opt => opt.MapFrom(dto => dto.Email))
            .ForMember("PasswordHash", opt => opt.MapFrom(dto => dto.Password));

        CreateMap<UserSettingsDto, User>();
        CreateMap<User, UserSettingsDto>();
        CreateMap<User, UserPublicDataDto>()
            .ForMember("Favourites", opt => opt.MapFrom(favourites => favourites.Favourites.Select(f => f.Id)))
            .ForMember("Reviews", opt => opt.MapFrom(reviews => reviews.Reviews.Select(r => r.Id)));;
        CreateMap<User, UserPrivateDataDto>()
            .ForMember("Purchased", opt => opt.MapFrom(user => user.Purchased.Select(p => p.Id)))
            .ForMember("Favourites", opt => opt.MapFrom(favourites => favourites.Favourites.Select(f => f.Id)))
            .ForMember("Reviews", opt => opt.MapFrom(reviews => reviews.Reviews.Select(r => r.Id)))
            .ForMember("Orders", opt => opt.MapFrom(reviews => reviews.Orders.Select(o => o.Id)));
    }
}