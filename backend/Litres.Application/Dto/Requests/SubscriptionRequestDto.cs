namespace Litres.Application.Dto.Requests;

public class SubscriptionRequestDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<string> GenresAllowed { get; set; }
}