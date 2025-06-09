using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Users;

public record GetUserByEmail(string Email) : IQuery<User>;