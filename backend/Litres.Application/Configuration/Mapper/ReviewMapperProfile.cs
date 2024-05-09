using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Domain.Entities;

namespace Litres.Application.Configuration.Mapper;

public class ReviewMapperProfile : Profile
{
    public ReviewMapperProfile()
    {
        CreateMap<ReviewCreateRequestDto, Review>();
    }
}