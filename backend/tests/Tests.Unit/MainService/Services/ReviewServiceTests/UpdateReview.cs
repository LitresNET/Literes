using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.ReviewServiceTests;

public class UpdateReview
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IReviewRepository> _reviewRepositoryMock = new();

    private ReviewService ReviewService => new(
        _unitOfWorkMock.Object
    );

    public UpdateReview()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Review>())
            .Returns(_reviewRepositoryMock.Object);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task DefaultReview_UpdateReview(long reviewId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var review = fixture
            .Build<Review>()
            .With(r => r.Id, reviewId)
            .Create();
        
        _reviewRepositoryMock
            .Setup(repository => repository.GetByIdAsync(review.Id))
            .ReturnsAsync(review);
        _reviewRepositoryMock
            .Setup(repository => repository.Update(review))
            .Verifiable();
            
        // Act
        await ReviewService.UpdateReview(review);

        // Assert
        _reviewRepositoryMock.Verify();
        //TODO: Добавить эту строчку для всех других методов, которые должны менять бд
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task NotExistentReview_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var review = fixture
            .Build<Review>()
            .With(r => r.Id, -1L)
            .Create();
        var expected = new EntityNotFoundException(typeof(Review), (-1L).ToString());
        
        _reviewRepositoryMock
            .Setup(repository => repository.Update(It.IsAny<Review>()))
            .Returns(It.IsAny<Review>());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => ReviewService.UpdateReview(review));

        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
    
    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var review = fixture
            .Build<Review>()
            .With(r => r.Id, -1L)
            .Create();
        var expected = new DbUpdateException();

        _reviewRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .Throws(new DbUpdateException());
        
        // Act
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => ReviewService.UpdateReview(review));

        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}