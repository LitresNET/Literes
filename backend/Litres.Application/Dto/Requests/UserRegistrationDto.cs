using System.ComponentModel.DataAnnotations;

namespace Litres.Application.Dto.Requests;

public class UserRegistrationDto
{
    [Required]
    [Length(3, 32)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Length(5, 256)]
    public string Email { get; set; }

    [Required]
    [Length(8, 128)]
    [RegularExpression(
        "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least 1 number, 1 upper-case character"
    )]
    public string Password { get; set; }
}
