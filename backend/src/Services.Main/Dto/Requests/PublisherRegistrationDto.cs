namespace backend.Dto.Requests;

public class PublisherRegistrationDto(string name, string email, string password, string contractNumber)
    : UserRegistrationDto(name, email, password)
{
    public required string ContractNumber { get; set; } = contractNumber;
}