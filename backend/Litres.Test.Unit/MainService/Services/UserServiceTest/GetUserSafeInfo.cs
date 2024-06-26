﻿using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserSafeInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(_publisherRepositoryMock.Object, _userRepositoryMock.Object);

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
}