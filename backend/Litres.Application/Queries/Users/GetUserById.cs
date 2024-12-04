using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Users;

public class GetUserById(long userId) : IQuery<User>
{
    public long UserId { get; } = userId;
}