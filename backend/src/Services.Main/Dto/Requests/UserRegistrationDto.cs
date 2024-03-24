namespace backend.Dto.Requests;

public class UserRegistrationDto(string Name, string Email, string Password)
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}