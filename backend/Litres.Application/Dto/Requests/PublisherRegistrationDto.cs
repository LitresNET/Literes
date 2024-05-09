using System.ComponentModel.DataAnnotations;

namespace Litres.Application.Dto.Requests;

public class PublisherRegistrationDto : UserRegistrationDto
{
    [Required]
    [MaxLength(256)]
    public string ContractNumber { get; set; }
}