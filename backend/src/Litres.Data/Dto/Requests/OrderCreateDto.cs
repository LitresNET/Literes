namespace Litres.Data.Dto.Requests;

public class OrderCreateDto
{
    public bool WasPaymentSuccessful { get; set; }
    public long OrderId { get; set; }
}