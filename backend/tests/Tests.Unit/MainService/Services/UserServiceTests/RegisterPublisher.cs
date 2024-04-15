﻿using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.MainService.Services.UserServiceTests;

public class RegisterPublisher
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock = new(
        Mock.Of<IUserStore<User>>(), 
        null, null, null, null, null, null, null, null);
    private readonly Mock<SignInManager<User>> _signInManagerMock = new(null, null, null, null, null, null);
    private readonly Mock<IPublisherRepository> _publisherRepository = new();
    private readonly Mock<RoleManager<IdentityRole<long>>> _roleManagerMock = new();
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    
    private UserService UserService => new(
        _unitOfWorkMock.Object,
        _userManagerMock.Object,
        _signInManagerMock.Object,
        _roleManagerMock.Object,
        _jwtTokenServiceMock.Object
    );

    public RegisterPublisher()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Publisher>())
            .Returns(_publisherRepository.Object);
    }
}