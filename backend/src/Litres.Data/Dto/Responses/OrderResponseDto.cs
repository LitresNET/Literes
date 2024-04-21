namespace Litres.Data.Dto.Responses;

public class OrderResponseDto
{
    public long OrderId { get; set; }
    public List<ProductResponseDto> Products { get; set; }
}