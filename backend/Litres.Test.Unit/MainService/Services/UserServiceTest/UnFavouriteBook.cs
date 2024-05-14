using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class UnFavouriteBook
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();

    private UserService UserService => new(
        _publisherRepositoryMock.Object,
        _userRepositoryMock.Object
        );
    
    [Theory]
    [InlineData(52)]
    [InlineData(1)]
    [InlineData(100)]
    public async Task DefaultUserWithExistingBook_ReturnsDeletedBook(long bookIdToDelete)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var rnd = new Random();
        var books = Enumerable.Range(1, 10)
            .Select(_ => new Book {Id = rnd.Next(1, (int) bookIdToDelete * 2 + 5)})
            .ToList();

        var user = fixture
            .Build<User>()
            .With(u => u.Favourites, books.Append(new Book {Id = bookIdToDelete}).ToList())
            .Create();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);

        var expected = books.Where(b => b.Id != bookIdToDelete).ToList();
        
        // Act 
        await UserService.UnFavouriteBookAsync(user.Id, bookIdToDelete);
        
        // Assert
        Assert.Equal(expected, user.Favourites);
    }
    
    [Fact]
    public async Task DefaultUserWithNotExistingBookInList_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 42L;
        const long bookIdToDelete = 42L;

        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var rnd = new Random();
        var books = Enumerable.Range(1, 10)
            .Select(_ => new Book {Id = rnd.Next(1, (int) bookIdToDelete * 2 + 5)})
            .ToList();

        var user = fixture
            .Build<User>()
            .With(u => u.Favourites, books.Where(b => b.Id != bookIdToDelete).ToList())
            .Create();
        
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);

        var expected = new EntityNotFoundException(typeof(Book), bookIdToDelete.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            UserService.UnFavouriteBookAsync(userId, bookIdToDelete));
        
        // Arrange
        Assert.Equal(expected.Message, actual.Message);
    }
}