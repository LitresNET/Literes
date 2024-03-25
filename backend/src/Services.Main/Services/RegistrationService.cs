using backend.Abstractions;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public class RegistrationService(IUserRepository userRepository, IPublisherRepository publisherRepository, 
    IUnitOfWork unitOfWork, UserManager<User> userManager) : IRegistrationService
{

    public async Task<IdentityResult> RegisterUserAsync(User user)
    { 
        var result = await userManager.CreateAsync(user);
        return result;
    }

    public async Task<IdentityResult> RegisterPublisherAsync(Publisher publisher)
    {
        throw new NotImplementedException();
    }
}