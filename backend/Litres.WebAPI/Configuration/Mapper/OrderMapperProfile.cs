using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.WebAPI.Configuration.Mapper;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<OrderDto, Order>()
            .ForMember(o => o.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)))
            .ForMember(o => o.OrderedBooks, opt => 
                opt.MapFrom(src => 
                    src.Books.Select(b => 
                        new BookOrder
                        {
                            BookId = b.BookId, 
                            Quantity = b.Amount
                        }).ToList()));

        CreateMap<Order, OrderDto>()
            .ForMember(dto => dto.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dto => dto.Books, opt => 
                opt.MapFrom(src => 
                    src.OrderedBooks.Select(orderBook => 
                        new ProductResponseDto
                        {
                            BookName = orderBook.Book.Name,
                            Amount = orderBook.Quantity,
                            Price = orderBook.Book.Price
                        })));
    }
}