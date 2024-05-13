using AutoFixture;
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
=======
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;
using Litres.Main.Services;
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserSafeInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs
    private UserService UserService => new(
        _publisherRepositoryMock.Object,
        _userRepositoryMock.Object
    );
=======
    private UserService UserService => new(_publisherRepositoryMock.Object, _userRepositoryMock.Object);
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[]
        {
            "dthcrvsh", "www.lalka.ru/zetochka", new List<Book>
            {
                new()
                {
                    Name = "Everlasting summer",
                    Description = "Best incel propaganda",
                    PublisherId = 3
                },
                new()
                {
                    Id = 1,
                    Name = "Fire punch",
                    Description = "cool",
                    PublisherId = 68
                }

            },
            new List<Review>
            {
                new()
                {
                    Id = 5,
                    Content = "Very bad",
                    Rating = 2,
                    UserId = 1,
                    BookId = 1
                },
                new()
                {
                    Id = 5,
                    Content = "Very bad",
                    Rating = 2,
                    UserId = 1,
                    BookId = 1,
                    ReviewLikes = new List<ReviewLike>
                    {
                        new()
                        {
                            UserId = 2,
                            IsLike = true
                        }
                    }
                }
            }
        };

        yield return new object[]
        {
            "Dead", "dead", null, null
        };

        yield return new object[]
        {
            "Dead", null, null, null
        };
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public async Task DefaultUser_ReturnsUser(string userName, string? avatarUrl,
        List<Book>? userFavourites, List<Review>? userReviews)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var expectedUser = fixture.Build<User>()
            .With(u => u.UserName, userName)
            .With(u => u.AvatarUrl, avatarUrl)
            .With(u => u.Favourites, userFavourites)
            .With(u => u.Reviews, userReviews)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedUser);

        var service = UserService;

        // Act
        var result = await service.GetUserByIdAsync(expectedUser.Id);

        // Assert
        Assert.Equal(expectedUser, result);
    }
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs

    [Fact]
    public async Task NotExistingUser_ThrowsOrderNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var user = fixture.Create<User>();

        var expected = new EntityNotFoundException(typeof(User), user.Id.ToString());

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await UserService.GetUserByIdAsync(user.Id)
        );

        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
=======
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserSafeInfo.cs
}