using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;

namespace Litres.Application.Configuration.Mapper;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<OrderRequestDto, Order>();
        
        CreateMap<OrderProcessDto, Order>()
            .ForMember(
                "OrderedBooks", 
                opt => 
                    opt.MapFrom(src => 
                        src.BooksAmount.Select(kv => 
                            new BookOrder
                            {
                                BookId = kv.Key, 
                                Quantity = kv.Value
                            }).ToList()));

        CreateMap<Order, OrderResponseDto>()
            .ForMember("OrderId", opt => 
                opt.MapFrom(src => src.Id))
            .ForMember("Products", opt => 
                opt.MapFrom(src => 
                    src.OrderedBooks.Select(orderBook => 
                        new ProductResponseDto
                        {
                            Name = orderBook.Book.Name,
                            Amount = orderBook.Quantity,
                            Price = orderBook.Book.Price
                        })));
    }
}