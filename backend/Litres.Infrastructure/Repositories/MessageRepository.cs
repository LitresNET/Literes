using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class MessageRepository(ApplicationDbContext appDbContext) 
    : Repository<Message>(appDbContext), IMessageRepository;