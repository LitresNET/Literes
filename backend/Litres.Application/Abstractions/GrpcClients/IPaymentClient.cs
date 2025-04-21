using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;

namespace Litres.Application.Abstractions.GrpcClients;

public interface IPaymentClient 
{
    public Task<CreateOrderResponseDto> RegisterPaymentAsync(CreateOrderDto request);
}