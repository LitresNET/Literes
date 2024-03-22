using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class AuthorRepository(ApplicationDbContext appDbContext) : IAuthorRepository
{
    public async Task<Author?> GetAuthorByIdAsync(long authorId)
    {
        return await appDbContext.Author.FirstOrDefaultAsync(author => author.Id == authorId);
    }
}