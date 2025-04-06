using Grpc.Core;
using Grpc.Net.Client;
using Litres.Application.Abstractions.GrpcClients;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Payment;

namespace Litres.Infrastructure.Grpc;

public class PaymentGrpcClient(string url) : IPaymentClient
{
    public async Task<CreateOrderResponseDto> RegisterPaymentAsync(CreateOrderDto request)
    {
        using var channel = GrpcChannel.ForAddress(url);
        var client = new PaymentRegistrationService.PaymentRegistrationServiceClient(channel);
        var grpcRequest = new PaymentRegistrationRequest()
        {
            OrderId = request.OrderId.ToString(),
            PaymentId = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            Currency = "RUB",
            ReturnUrl = "",
            CancelUrl = ""
        };
        
        try
        {
            var response = await client.RegisterPaymentAsync(grpcRequest);
            return new CreateOrderResponseDto {Success = response.Success, PaymentRedirectUrl = response.PaymentUrl};
        }
        catch (RpcException ex)
        {
            throw new Exception($"gRPC error: {ex.Status.Detail}", ex);
        }
    }
}