using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Entities;

namespace Litres.Application.Configuration.Mapper;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookCreateRequestDto, Book>();
        CreateMap<BookUpdateRequestDto, Book>();
        CreateMap<Book, BookResponseDto>()
            .ForMember(dto => dto.Author, opt => opt.MapFrom(book => book.Author.Name))
            .ForMember(dto => dto.Series, opt => opt.MapFrom(book => book.Series.Name))
            .ForMember(dto => dto.Publisher, opt => opt.MapFrom(book => book.Publisher.User.Name));
    }
}