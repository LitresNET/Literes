namespace Litres.Data.Dto.Responses;

public class OrderCreateResponseDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public long PickupPointId { get; set; }
    public long UserId { get; set; }
}