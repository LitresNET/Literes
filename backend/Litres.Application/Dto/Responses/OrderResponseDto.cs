namespace Litres.Application.Dto.Responses;

public class OrderResponseDto
{
    public long OrderId { get; set; }
    public List<ProductResponseDto> Products { get; set; }
}