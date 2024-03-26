namespace backend.Exceptions;

public class BookNotFoundException(long bookId) : Exception($"Book {bookId} was not found");