using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Requests;

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
    [Length(8,128)]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    public string Password { get; set; }
}