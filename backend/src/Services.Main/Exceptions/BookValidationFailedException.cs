namespace backend.Exceptions;

public class BookValidationFailedException(string property) : Exception($"Book property {property} is required");