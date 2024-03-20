using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class SeriesRepository(ApplicationDbContext appDbContext) : ISeriesRepository
{
    public async Task<Series?> GetSeriesByIdAsync(long seriesId)
    {
        return await appDbContext.Series.FirstOrDefaultAsync(series => series.Id == seriesId);
    }
}