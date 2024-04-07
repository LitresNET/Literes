﻿using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.MainService.Services.UserServiceTests;

public class RegisterUser
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IConfiguration> _configurationMock = new();

    private UserService UserService => new(
        _unitOfWorkMock.Object,
        _userManagerMock.Object,
        _configurationMock.Object
    );
    
    
}