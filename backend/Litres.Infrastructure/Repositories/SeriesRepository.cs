using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class SeriesRepository(ApplicationDbContext appDbContext) 
    : Repository<Series>(appDbContext), ISeriesRepository;