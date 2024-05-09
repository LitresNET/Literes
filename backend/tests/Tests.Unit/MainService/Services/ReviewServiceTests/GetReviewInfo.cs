using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.ReviewServiceTests;

public class GetReviewInfo
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IReviewRepository> _reviewRepositoryMock = new();

    private ReviewService ReviewService => new(
        _unitOfWorkMock.Object
    );

    public GetReviewInfo()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Review>())
            .Returns(_reviewRepositoryMock.Object);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task DefaultReview_ReturnsReview(long reviewId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expected = fixture
            .Build<Review>()
            .With(r => r.Id, reviewId)
            .Create();
        
        _reviewRepositoryMock
            .Setup(repository => repository.GetByIdAsync(reviewId))
            .ReturnsAsync(expected);
        
        // Act
        var actual = await ReviewService.GetReviewInfo(reviewId);

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task NotExistentReviewId_ThrowsEntityNotFoundException()
    {
        // Arrange
        var expected = new EntityNotFoundException(typeof(Review), (-1L).ToString());
        
        _reviewRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(It.IsAny<Review>());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => ReviewService.GetReviewInfo(-1L));

        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
    
    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var expected = new DbUpdateException();

        _reviewRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .Throws(new DbUpdateException());
        
        // Act
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => ReviewService.GetReviewInfo(100));

        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}