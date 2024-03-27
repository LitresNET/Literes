using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class SeriesRepository(ApplicationDbContext appDbContext) : ISeriesRepository
{
    public async Task<Series?> GetSeriesByIdAsync(long seriesId)
    {
        return await appDbContext.Series.FirstOrDefaultAsync(series => series.Id == seriesId);
    }
}