using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;

namespace Litres.Application.Configuration.Mapper;

public class RequestMapperProfile : Profile
{
    public RequestMapperProfile()
    {
        CreateMap<Request, RequestResponseDto>()
            .ForMember(dto => dto.RequestId, opt => opt.MapFrom(r => r.Id));
    }
}