using System.Text;
using Litres.Data.Abstractions.Services;
using Newtonsoft.Json;

namespace Litres.Main.Services;

public class PaymentService(IConfiguration configuration) : IPaymentService
{
    private string PaymentUrl => configuration["PaymentServiceUrl"]!;
    private readonly HttpClient _httpClient = new();

    public async Task ReplenishBalance(long userId, decimal amount)
    {
        var response = await _httpClient.PostAsync(PaymentUrl, new StringContent(JsonConvert.SerializeObject(new { Amount = amount }), Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
        }
    }
}