namespace Litres.Data.Dto.Requests;

public class OrderCreateRequestDto
{
    public long UserId { get; set; }
    public long PickupPointId { get; set; }
    public string Description { get; set; }
}