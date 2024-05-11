namespace Litres.Application.Dto.Responses;

public class ProductResponseDto
{
    public long BookId { get; set; }
    public string BookName { get; set; }
    public int Amount { get; set; }
    public int Price { get; set; }
}