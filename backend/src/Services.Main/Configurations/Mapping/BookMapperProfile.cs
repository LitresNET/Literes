using AutoMapper;
using backend.Dto.Requests;
using backend.Models;

namespace backend.Configurations.Mapping;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookCreateRequestDto, Book>();
    }
}