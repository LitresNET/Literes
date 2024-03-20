namespace backend.Exceptions;

public class BookLackRequiredPropertiesException(string property) : Exception($"Property {property} is required");