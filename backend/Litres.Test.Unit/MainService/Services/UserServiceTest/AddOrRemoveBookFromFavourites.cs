using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Litres.Infrastructure.Repositories;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class AddOrRemoveBookFromFavourites
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();

    private UserService UserService => new(
        _publisherRepositoryMock.Object,
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object
        );
    
    [Theory]
    [InlineData(52)]
    [InlineData(1)]
    [InlineData(100)]
    public async Task DefaultUserWithExistingBookInFavourites_ReturnsDeletedBook(long bookId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var rnd = new Random();
        var books = Enumerable.Range(1, 10)
            .Select(_ => new Book {Id = rnd.Next(1, (int) bookId * 2 + 5)})
            .Where(b => b.Id != bookId)
            .ToList();

        var user = fixture
            .Build<User>()
            .With(u => u.Favourites, books.Append(new Book {Id = bookId}).ToList())
            .Create();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        var expected = books.Where(b => b.Id != bookId).ToList();
        
        // Act 
        await UserService.AddOrRemoveBookFromFavouritesAsync(user.Id, bookId);
        
        // Assert
        Assert.Equal(expected, user.Favourites);
    }
    
    [Theory]
    [InlineData(52)]
    [InlineData(1)]
    [InlineData(100)]
    public async Task DefaultUserWithNotExistingBookInFavourites_ReturnsAddedBook(long bookId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var rnd = new Random();
        var books = Enumerable.Range(1, 10)
            .Select(_ => new Book {Id = rnd.Next(1, (int) bookId * 2 + 5)})
            .Where(b => b.Id != bookId)
            .ToList();
        var addedBook = new Book { Id = bookId };

        var user = fixture
            .Build<User>()
            .With(u => u.Favourites, books.ToList())
            .Create();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(addedBook);

        var expected = books.Append(addedBook).ToList();
        
        // Act 
        await UserService.AddOrRemoveBookFromFavouritesAsync(user.Id, bookId);
        
        // Assert
        Assert.Equal(expected, user.Favourites);
    }
}