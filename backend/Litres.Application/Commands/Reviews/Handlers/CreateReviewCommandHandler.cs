using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Reviews.Handlers;

public class CreateReviewCommandHandler(
    IReviewRepository reviewRepository,
    IBookRepository bookRepository,
    IMapper mapper
    ) : ICommandHandler<CreateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> HandleAsync(CreateReviewCommand command)
    {
        var review = mapper.Map<Review>(command);
        
        // у отзыва либо отсутствует ссылка на родительский отзыв, либо на книгу, иначе - ошибка
        if (review.BookId is null && review.ParentReviewId is null ||
            review.BookId is not null && review.ParentReviewId is not null)
            throw new EntityUnprocessableException(typeof(Review), review.Id.ToString(),
                "no parent review and book that it belongs to.");

        if (review.ParentReviewId is not null)
            await reviewRepository.GetByIdAsNoTrackingAsync((long) review.ParentReviewId!);
        
        if (review.BookId is not null)
            await bookRepository.GetByIdAsNoTrackingAsync((long) review.BookId!);

        var dbBook = await bookRepository.GetByIdAsNoTrackingAsync((long) review.BookId!);
        if (dbBook.Reviews!.Any(r => r.UserId == review.UserId && r.BookId is not null))
            throw new EntityUnprocessableException(typeof(Review), review.Id.ToString(),
                "user already left review on that book");
        
        var dbReview = await reviewRepository.AddAsync(review);
        await reviewRepository.SaveChangesAsync();
        
        return mapper.Map<ReviewDto>(dbReview);
    }
}