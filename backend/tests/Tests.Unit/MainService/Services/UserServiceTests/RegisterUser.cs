using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.MainService.Services.UserServiceTests;

public class RegisterUser
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock = new(
        Mock.Of<IUserStore<User>>(), 
        null, null, null, null, null, null, null, null);

    private readonly Mock<SignInManager<User>> _signInManagerMock = new(null, null, null, null, null, null);
    
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();

    private UserService UserService => new(
        _unitOfWorkMock.Object,
        _userManagerMock.Object,
        _signInManagerMock.Object,
        _jwtTokenServiceMock.Object
    );
    
    
    
}