using Litres.Domain.Entities;

namespace Litres.Application.Dto.Responses;

public class PublisherStatisticsDto
{
    public Publisher Publisher { get; set; }
    public int PublishedBookCount { get; set; }
    public int OwnedBookCount { get; set; }
}