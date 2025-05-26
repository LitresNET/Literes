using System.Data.Entity;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetUserByEmailHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUserByEmail, User>
{
    public Task<User> HandleAsync(GetUserByEmail q)
    {
        var result = dbContext.User.FirstOrDefault(u => u.Email == q.Email);
        return Task.FromResult(result ?? new User());
    }
}