using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class ChatRepository(ApplicationDbContext appDbContext) 
    : Repository<Chat>(appDbContext), IChatRepository
{
    
}