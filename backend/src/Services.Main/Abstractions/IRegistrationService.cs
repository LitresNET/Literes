using backend.Dto.Responses;
using backend.Models;
using Microsoft.Identity.Client;


namespace backend.Abstractions;

public interface IRegistrationService
{
    public Task<RegisterUserResponseDto> RegisterUserAsync(User user);

    public Task<RegisterUserResponseDto> RegisterPublisherAsync(Publisher publisher);
}