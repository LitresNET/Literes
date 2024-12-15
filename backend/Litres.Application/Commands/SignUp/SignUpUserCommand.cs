using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Commands.SignUp;

public class SignUpUserCommand(string name, string email, string password, string role = "Member") : ICommand<IdentityResult>
{
    [Required]
    [Length(3, 32)]
    public string Name { get; set; } = name;

    [Required]
    [EmailAddress]
    [Length(5, 256)]
    public string Email { get; set; } = email;

    [Required]
    [Length(8, 128)]
    [RegularExpression(
        "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least 1 number, 1 upper-case character"
    )]
    public string Password { get; set; } = password;
    
    [JsonIgnore] //Раз мы делаем отдельный метод в контроллере под каждую роль, это поле не должно быть видно "снаружи"
    [AllowedValues("Admin", "Publisher", "Member", "Agent")]
    public string Role { get; set; } = role;
}