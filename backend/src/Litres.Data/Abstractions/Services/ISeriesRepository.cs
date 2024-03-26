using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface ISeriesRepository
{
    public Task<Series?> GetSeriesByIdAsync(long seriesId);
}