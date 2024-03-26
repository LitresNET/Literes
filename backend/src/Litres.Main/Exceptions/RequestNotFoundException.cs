namespace Litres.Main.Exceptions;

public class RequestNotFoundException(long requestId) : Exception($"Request {requestId} was not found");