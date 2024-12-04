using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetUserByIdQueryHandler(
    ApplicationDbContext context
    ) : IQueryHandler<GetUserById, User>
{
    public async Task<User> HandleAsync(GetUserById q)
    {
        var user = await context.User.FirstOrDefaultAsync(e => e.Id == q.UserId);
        if (user is null)
            throw new EntityNotFoundException(typeof(User), q.UserId.ToString());
        
        return user;
    }
}