using AutoMapper;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class PublisherMapperProfile : Profile
{
    public PublisherMapperProfile()
    {
        CreateMap<Publisher, PublisherStatisticsDto>()
            .ForMember("Publisher", opt => opt.MapFrom(publisher => publisher))
            .ForMember("PublishedBookCount", opt => opt.MapFrom(publisher => publisher.Books.Count))
            .ForMember("OwnedBookCount", opt => opt.MapFrom(publisher => publisher.Books.Select(b => b.Count).Sum()));
    }
   
}