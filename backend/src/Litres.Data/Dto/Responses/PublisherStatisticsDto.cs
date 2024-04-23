using Litres.Data.Models;

namespace Litres.Data.Dto.Responses;

public class PublisherStatisticsDto
{
    public Publisher Publisher { get; set; }
    public int PublishedBookCount { get; set; }
    public int OwnedBookCount { get; set; }
}