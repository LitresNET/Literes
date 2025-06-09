using HotChocolate;
using HotChocolate.Data;
using Litres.Domain.Entities;
using Litres.Infrastructure;

namespace Litres.WebAPI.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetAccount([Service] ApplicationDbContext ctxt, long userId)
    {
        return ctxt.User.Where(u => u.Id == userId);
    }
}