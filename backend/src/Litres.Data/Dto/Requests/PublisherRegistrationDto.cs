using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Dto.Requests;

public class PublisherRegistrationDto : UserRegistrationDto
{
    [Required]
    [MaxLength(256)]
    public string ContractNumber { get; set; }
}