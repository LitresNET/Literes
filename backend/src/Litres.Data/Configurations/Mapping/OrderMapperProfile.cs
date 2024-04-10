using AutoMapper;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<OrderCreateRequestDto, Order>();
        CreateMap<Order, OrderCreateResponseDto>();
    }
}