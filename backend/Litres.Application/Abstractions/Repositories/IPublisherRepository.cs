﻿using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IPublisherRepository : IRepository<Publisher>
{
    public Task<Publisher> GetByLinkedUserIdAsync(long userId);
}