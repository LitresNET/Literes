namespace Litres.Application.Dto.Responses;

public class CreateOrderResponseDto
{
    public bool Success { get; set; }
    public string PaymentRedirectUrl { get; set; }
}