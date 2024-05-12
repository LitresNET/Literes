using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class NotificationRepository(ApplicationDbContext appDbContext) 
    : Repository<Notification>(appDbContext), INotificationRepository;