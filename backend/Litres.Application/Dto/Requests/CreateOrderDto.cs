namespace Litres.Application.Dto.Requests;

public class CreateOrderDto
{
    public long OrderId { get; set; }
    public double Amount { get; set; }
}