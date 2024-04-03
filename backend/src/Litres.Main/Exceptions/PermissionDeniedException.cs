namespace Litres.Main.Exceptions;

public class PermissionDeniedException(string source) : Exception($"Access denied for {source}");