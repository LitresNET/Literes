namespace Litres.Domain.Exceptions;

public class AdditionalRegistrationRequiredException(string info)
    : Exception("Authorization failed. Info: " + info);