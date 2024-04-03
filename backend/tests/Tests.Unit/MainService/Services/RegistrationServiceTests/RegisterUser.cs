using Litres.Data.Abstractions.Repositories;
using Moq;

namespace Tests.MainService.Services.RegistrationServiceTests;

public class RegisterUser
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepository = new();


    public RegisterUser()
    {

    }
}