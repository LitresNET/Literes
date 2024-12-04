using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.SignIn;

public class SignInUserCommand(string email, string password) : ICommand<string>
{
    [Required]
    [EmailAddress]
    [Length(5, 256)]
    public string Email { get; set; } = email;

    [Required]
    [Length(8, 128)]
    public string Password { get; set; } = password;
}