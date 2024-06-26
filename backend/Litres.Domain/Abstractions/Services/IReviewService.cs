﻿using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IReviewService
{
    public Task<Review> GetReviewAsync(long reviewId);
    public Task<List<Review>> GetReviewListByBookIdAsync(long bookId, int page);
    public Task<Review> AddReview(Review review);
    public Task RateReview(long reviewId, long userId, bool isLike);
    public Task RemoveReviewRate(long reviewId, long userId);
}