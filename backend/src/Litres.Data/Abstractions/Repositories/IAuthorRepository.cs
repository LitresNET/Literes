using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IAuthorRepository
{
    public Task<Author?> GetAuthorByIdAsync(long authorId);
}