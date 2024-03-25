namespace backend.Dto.Requests;

public class PublisherRegistrationDto : UserRegistrationDto
{
    public required string ContractNumber { get; set; }
}