namespace Litres.Main.Exceptions;

public class UserPermissionDeniedException(string source) : Exception($"You don't have rights to access source {source}");