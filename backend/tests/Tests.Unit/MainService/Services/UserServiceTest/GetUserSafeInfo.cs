using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;
using Litres.Main.Services;
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
            "dthcrvsh", "www.lalka.ru/zetochka", new List<Book>()
            {
                new Book()
                {
                    Name = "Everlasting summer",
                    Description = "Best incel propaganda",
                    PublisherId = 3
                },
                new Book()
                {
                    Id = 1,
                    Name = "Fire punch",
                    Description = "cool",
                    PublisherId = 68
                }

            },
            new List<Review>()
            {
                new Review()
                {
                    Id = 5,
                    Content = "Very bad",
                    Rating = 2,
                    UserId = 1,
                    BookId = 1
                },
                new Review()
                {
                    Id = 5,
                    Content = "Very bad",
                    Rating = 2,
                    UserId = 1,
                    BookId = 1,
                    ReviewLikes = new List<ReviewLike>()
                    {
                        new ReviewLike()
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
            .With(u => u.Name, userName)
            .With(u => u.AvatarUrl, avatarUrl)
            .With(u => u.Favourites, userFavourites)
            .With(u => u.Reviews, userReviews)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedUser);
        
        var service = UserService;

        // Act
        var result = await service.GetUserInfoAsync(expectedUser.Id);

        // Assert
        Assert.Equal(expectedUser, result);
    }
}