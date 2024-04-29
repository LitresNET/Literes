using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Data.Repositories;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.ReviewServiceTests;

public class DeleteReview
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IReviewRepository> _reviewRepositoryMock = new();

    private ReviewService ReviewService => new(
        _unitOfWorkMock.Object
        );

    public DeleteReview()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Review>())
            .Returns(_reviewRepositoryMock.Object);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task DefaultReview_DeleteReviewById(long reviewId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var review = fixture
            .Build<Review>()
            .With(r => r.Id, reviewId)
            .Create();
        
        _reviewRepositoryMock
            .Setup(repo => repo.GetByIdAsync(reviewId))
            .ReturnsAsync(review);
        _reviewRepositoryMock.Setup(repository => repository.Delete(review)).Verifiable();


        // Act
        await ReviewService.DeleteReview(reviewId);

        // Assert
        _reviewRepositoryMock.Verify();
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task DefaultReview_DeleteReview(long reviewId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var review = fixture
            .Build<Review>()
            .With(r => r.Id, reviewId)
            .Create();
        
        _reviewRepositoryMock
            .Setup(repository => repository.Delete(review))
            .Verifiable();

        // Act
        await ReviewService.DeleteReview(review);

        // Assert
        _reviewRepositoryMock.Verify();
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
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
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => ReviewService.DeleteReview(-1L));

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
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => ReviewService.DeleteReview(100));

        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}