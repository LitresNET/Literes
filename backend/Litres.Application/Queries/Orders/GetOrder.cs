using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Orders;

public record GetOrder(long OrderId) : IQuery<OrderDto>;