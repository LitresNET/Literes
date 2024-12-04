using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;

namespace Litres.WebAPI.Configuration.Mapper;

public class PublisherMapperProfile : Profile
{
    public PublisherMapperProfile()
    {
        CreateMap<Publisher, PublisherStatisticsDto>()
            .ForMember("PublisherId", opt => opt.MapFrom(publisher => publisher.UserId))
            .ForMember("Books", opt => opt.MapFrom(publisher => publisher.Books))
            .ForMember("PublishedBookCount", opt => opt.MapFrom(publisher => publisher.Books.Count))
            .ForMember("OwnedBookCount", opt => opt.MapFrom(publisher => publisher.Books.Select(b => b.Count).Sum()));
    }
}