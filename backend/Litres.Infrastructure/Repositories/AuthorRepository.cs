using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class AuthorRepository(ApplicationDbContext appDbContext) 
    : Repository<Author>(appDbContext), IAuthorRepository;