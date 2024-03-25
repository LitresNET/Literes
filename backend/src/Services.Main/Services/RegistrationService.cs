using backend.Abstractions;
using backend.Dto.Responses;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class RegistrationService(IUserRepository userRepository, IPublisherRepository publisherRepository, IUnitOfWork unitOfWork, UserManager<User> userManager) : IRegistrationService
{

    public async Task<RegisterUserResponseDto> RegisterUserAsync(User user)
    { 
        
        try
        { 
            await userRepository.AddNewUserAsync(user);
            await unitOfWork.SaveChangesAsync();
            return new RegisterUserResponseDto
            {
                IsSuccess = true,
                Message = "Success"
            };
        }
        catch (DbUpdateException e)
        {
            return new RegisterUserResponseDto
            {
                IsSuccess = false,
                Message = e.Message
            };
        }

    }

    public async Task<RegisterUserResponseDto> RegisterPublisherAsync(Publisher publisher)
    {
        throw new NotImplementedException();
    }
}