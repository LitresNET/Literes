using AutoMapper;
using Litres.Application.Commands.Reviews;
using Litres.Application.Dto;
using Litres.Domain.Entities;

namespace Litres.WebAPI.Configuration.Mapper;

public class ReviewMapperProfile : Profile
{
    public ReviewMapperProfile()
    {
        CreateMap<ReviewDto, Review>();

        CreateMap<CreateReviewCommand, Review>();
        
        CreateMap<Review, ReviewDto>()
            .ForMember(dto => dto.Likes, opt =>
                opt.MapFrom(src => src.ReviewLikes.Count(rl => rl.IsLike)))
            .ForMember(dto => dto.Dislikes, opt =>
                opt.MapFrom(src => src.ReviewLikes.Count(rl => !rl.IsLike)));
        
        CreateMap<Review, CreateReviewCommand>()
            .ForMember(dto => dto.Likes, opt =>
                opt.MapFrom(src => src.ReviewLikes.Count(rl => rl.IsLike)))
            .ForMember(dto => dto.Dislikes, opt =>
                opt.MapFrom(src => src.ReviewLikes.Count(rl => !rl.IsLike)));
    }
}