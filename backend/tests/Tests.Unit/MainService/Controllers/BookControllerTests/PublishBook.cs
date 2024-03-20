using AutoFixture;
using backend.Abstractions;
using backend.Controllers;
using backend.Models;
using Moq;

namespace Tests.MainService.Controllers.BookControllerTests;

public class PublishBook
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IBookService> _bookServiceMock = new();
    
    [Fact]
    public async Task DefaultBook_ReturnsRequestCreate_200()
    {
        // Arrange
        var book = _fixture.Build<Book>().Without(b => b.Requests).Create();
        var expectedRequest = new Request();

        _bookServiceMock.Setup(service => service.PublishNewBookAsync(book))
            .ReturnsAsync(expectedRequest);

        var controller = new BookController(_bookServiceMock.Object);

        // Act
        var response = await controller.PublishBook(book);
        var result = response.Value;

        // Assert
        Assert.Equal(expectedRequest, result);
        _bookServiceMock.Verify(service => service.PublishNewBookAsync(book), Times.Once);
    }

    public async Task EmptyBook_ReturnsBookLackRequiredPropertiesException_422()
    {
        
    }

    public async Task BookWithNotExistingAuthor_ReturnsAuthorNotFoundException_404()
    {
        
    }
    
    public async Task BookWithNotExistingSeries_ReturnsSeriesNotFoundException_404()
    {
        
    }
    
    public async Task DatabaseShut_ReturnsStorageUnavailableException_503()
    {
        
    }
}