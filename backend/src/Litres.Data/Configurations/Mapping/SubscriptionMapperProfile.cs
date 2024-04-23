using AutoMapper;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class SubscriptionMapperProfile : Profile
{
    public SubscriptionMapperProfile()
    {
        CreateMap<SubscriptionRequestDto, Subscription>()
            .ForMember(s => s.BooksAllowed, 
                opt => opt.MapFrom(dto => dto.GenresAllowed.Select(s => (Genre) Enum.Parse(typeof(Genre), s)))
            );
        CreateMap<Subscription, SubscriptionResponseDto>()
            .ForMember(dto => dto.BooksAllowed, opt => opt.MapFrom(s => s.BooksAllowed.Select(b => b.ToString())));
    }
}