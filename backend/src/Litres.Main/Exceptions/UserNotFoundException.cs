namespace Litres.Main.Exceptions;

public class UserNotFoundException(string email) : Exception($"No user with {email} email was found");