using AutoMapper;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;

namespace Litres.Data.Configurations.Mapping;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookCreateRequestDto, Book>();
        CreateMap<BookUpdateRequestDto, Book>();
    }
}