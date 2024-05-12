using Litres.Application.Dto.Responses;

namespace Litres.Application.Dto;

public class OrderDto
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long PickupPointId { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? ExpectedDeliveryTime { get; set; }
    public List<ProductResponseDto> Books { get; set; }
}