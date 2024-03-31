using AutoMapper;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class RequestMapperProfile : Profile
{
    public RequestMapperProfile()
    {
        CreateMap<Request, RequestResponseDto>()
            .ForMember(dto => dto.RequestId, opt => opt.MapFrom(r => r.Id));
    }
}