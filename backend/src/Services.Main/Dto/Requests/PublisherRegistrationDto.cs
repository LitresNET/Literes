using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Requests;

public class PublisherRegistrationDto : UserRegistrationDto
{
    [Required]
    [MaxLength(256)]
    public string ContractNumber { get; set; }
}