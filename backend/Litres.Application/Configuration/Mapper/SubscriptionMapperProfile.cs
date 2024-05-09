using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Configuration.Mapper;

public class SubscriptionMapperProfile : Profile
{
    public SubscriptionMapperProfile()
    {
        CreateMap<SubscriptionRequestDto, Subscription>()
            .ForMember(s => s.BooksAllowed, 
                opt => opt.MapFrom(dto => dto.GenresAllowed.Select(s => (GenreType) Enum.Parse(typeof(GenreType), s)))
            );
        CreateMap<Subscription, SubscriptionResponseDto>()
            .ForMember(dto => dto.BooksAllowed, opt => opt.MapFrom(s => s.BooksAllowed.Select(b => b.ToString())));
    }
}