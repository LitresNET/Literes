namespace Litres.Data.Exceptions;

public class PermissionDeniedException(string source) : Exception($"Access denied for {source}");