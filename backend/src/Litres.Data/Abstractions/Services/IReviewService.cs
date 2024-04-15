﻿using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IReviewService
{
    public Task AddReview(Review review);
    public Task LikeReview(long reviewId, long userId);
    public Task DislikeReview(long reviewId, long userId);
}