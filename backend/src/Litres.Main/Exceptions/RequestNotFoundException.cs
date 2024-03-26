namespace backend.Exceptions;

public class RequestNotFoundException(long requestId) : Exception($"Request {requestId} was not found");