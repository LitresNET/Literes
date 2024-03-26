namespace Litres.Main.Exceptions;

public class AuthorNotFoundException(long authorId) : Exception($"Author {authorId} was not found");