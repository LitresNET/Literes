using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface ISeriesRepository
{
    public Task<Series?> GetSeriesByIdAsync(long seriesId);
}