using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Moq;

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
}