using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class AuthorRepository(ApplicationDbContext appDbContext) : IAuthorRepository
{
    public async Task<Author?> GetAuthorByIdAsync(long authorId)
    {
        return await appDbContext.Author.FirstOrDefaultAsync(author => author.Id == authorId);
    }
}