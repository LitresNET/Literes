namespace Litres.Application.Dto.Requests;

public class OrderCreateDto
{
    public bool WasPaymentSuccessful { get; set; }
    public long OrderId { get; set; }
}