using backend.Models;

namespace backend.Abstractions;

public interface IAuthorRepository
{
    public Task<Author?> GetAuthorByIdAsync(long authorId);
}