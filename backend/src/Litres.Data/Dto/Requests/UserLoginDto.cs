using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Dto.Requests;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    [Length(5, 256)]
    public string Email { get; set; }
    [Required]
    [Length(8, 128)]
    public string Password { get; set; }
}