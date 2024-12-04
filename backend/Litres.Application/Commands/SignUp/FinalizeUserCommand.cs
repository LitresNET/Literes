using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Commands.SignUp;

public record FinalizeUserCommand(string name, string email, string password, string role = "Member")
    : ICommand<IdentityResult>
{
    [Required] [Length(3, 32)] public string Name { get; set; } = name;

    [Required]
    [EmailAddress]
    [Length(5, 256)]
    public string Email { get; set; } = email;

    [Required]
    [Length(8, 128)]
    [RegularExpression(
        "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
        ErrorMessage =
            "Password must be at least 6 characters long and contain at least 1 number, 1 upper-case character"
    )]
    public string Password { get; set; } = password;

    [AllowedValues("Admin", "Publisher", "Member", "Agent")]
    public string Role { get; set; } = role;
}