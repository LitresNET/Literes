using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Users;

public record GetOrderList(long UserId) : IQuery<IEnumerable<OrderDto>>;