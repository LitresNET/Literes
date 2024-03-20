using backend.Models;

namespace backend.Abstractions;

public interface ISeriesRepository
{
    public Task<Series?> GetSeriesByIdAsync(long seriesId);
}