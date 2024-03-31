using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class SeriesRepository(ApplicationDbContext appDbContext) : ISeriesRepository
{
    public async Task<Series> AddAsync(Series series)
    {
        var result = await appDbContext.Series.AddAsync(series);
        return result.Entity;
    }

    public Series Update(Series series)
    {
        var result = appDbContext.Series.Update(series);
        return result.Entity;
    }

    public Series Delete(Series series)
    {
        var result = appDbContext.Series.Remove(series);
        return result.Entity;
    }

    public async Task<Series?> GetByIdAsync(long seriesId)
    {
        return await appDbContext.Series.FirstOrDefaultAsync(series => series.Id == seriesId);
    }
}