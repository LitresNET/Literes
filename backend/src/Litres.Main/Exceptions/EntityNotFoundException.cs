namespace Litres.Main.Exceptions;

public class EntityNotFoundException<TEntity>(string? key) 
    : Exception($"{typeof(TEntity)} {key} was not found");