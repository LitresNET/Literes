namespace Litres.Data.Dto.Requests;

public class OrderRequestDto
{
    public long UserId { get; set; }
    public long PickupPointId { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
}