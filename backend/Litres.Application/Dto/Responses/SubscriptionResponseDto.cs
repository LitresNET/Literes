namespace Litres.Application.Dto.Responses;

public class SubscriptionResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public List<string> BooksAllowed { get; set; }
}