using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Orders;

public record GetOrder(long OrderId) : IQuery<OrderDto>;