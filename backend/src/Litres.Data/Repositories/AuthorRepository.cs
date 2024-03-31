using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class AuthorRepository(ApplicationDbContext appDbContext) : IAuthorRepository
{
    public async Task<Author> AddAsync(Author author)
    {
        var result = await appDbContext.Author.AddAsync(author);
        return result.Entity;
    }

    public Author Update(Author author)
    {
        var result = appDbContext.Author.Update(author);
        return result.Entity;
    }

    public Author Delete(Author author)
    {
        var result = appDbContext.Author.Remove(author);
        return result.Entity;
    }

    public async Task<Author?> GetByIdAsync(long authorId)
    {
        return await appDbContext.Author.FirstOrDefaultAsync(author => author.Id == authorId);
    }
}