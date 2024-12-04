using Litres.Domain.Entities;

namespace Litres.Application.Dto.Responses;

public class PublisherStatisticsDto
{
    public long PublisherId { get; set; }
    public List<BookResponseDto> Books { get; set; }
    public int PublishedBookCount { get; set; }
    public int OwnedBookCount { get; set; }
}