using AutoMapper;
using Litres.Application.Dto;
using Litres.Domain.Entities;
using Litres.Application.Dto.Responses;

namespace Litres.WebAPI.Configuration.Mapper;

public class ChatMapperProfile : Profile
{
    public ChatMapperProfile()
    {
        CreateMap<Chat, ChatPreviewDto>()
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(chat => chat.UserId))
            .ForMember(dto => dto.Username, opt => opt.MapFrom(chat => chat.User.Name))
            .ForMember(dto => dto.LastMessageDate, opt => opt.MapFrom(chat => 
                chat.Messages.OrderByDescending(m => m.SentDate).FirstOrDefault() != null 
                    ? chat.Messages.OrderByDescending(m => m.SentDate).First().SentDate 
                    : DateTime.MinValue));
        
        CreateMap<IEnumerable<Message>, ChatHistoryDto>()
            .ForMember(dto => dto.IsSuccess, opt => opt.MapFrom(src => src != null && src.Any()))  
            .ForMember(dto => dto.Messages, opt => opt.MapFrom(src => 
                src != null ? src.Select(m => new MessageDto 
                { 
                    Text = m.Text, 
                    From = m.From, 
                    SentDate = m.SentDate 
                }).ToList() : new List<MessageDto>()));  
        
    }
}

