using System.Text.Json.Serialization;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Orders;

public record DeleteOrderCommand(long OrderId) : ICommand<OrderDto>
{
    [JsonIgnore]
    public long UserId { get; set; }
}