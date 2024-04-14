namespace Litres.Data.Dto.Responses;

public class OrderResponseDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public long PickupPointId { get; set; }
    public long UserId { get; set; }
}