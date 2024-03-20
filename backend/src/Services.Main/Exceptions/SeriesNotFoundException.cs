namespace backend.Exceptions;

public class SeriesNotFoundException(long seriesId) : Exception($"Series {seriesId} was not found");