using AutoMapper;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class ReviewMapperProfile : Profile
{
    public ReviewMapperProfile()
    {
        CreateMap<ReviewCreateRequestDto, Review>();
    }
}