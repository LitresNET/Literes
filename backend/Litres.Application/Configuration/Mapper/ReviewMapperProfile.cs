using AutoMapper;
using Litres.Application.Dto;
using Litres.Domain.Entities;

namespace Litres.Application.Configuration.Mapper;

public class ReviewMapperProfile : Profile
{
    public ReviewMapperProfile()
    {
        CreateMap<ReviewDto, Review>();
        CreateMap<Review, ReviewDto>();
    }
}