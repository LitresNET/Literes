using backend.Abstractions;
using backend.Configurations;
using backend.Dto.Responses;
using backend.Models;

namespace backend.Services;

public class RegistrationService(ApplicationDbContext appDbContext, IUnitOfWork unitOfWork) : IRegistrationService
{
    public async Task<RegisterUserResponseDto> RegisterUserAsync(User user, IUserRepository userRepository)
    {
        throw new NotImplementedException();
    }

    public async Task<RegisterUserResponseDto> RegisterPublisherAsync(Publisher publisher, IPublisherRepository publisherRepository)
    {
        throw new NotImplementedException();
    }
}