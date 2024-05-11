using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(
        IReviewService service,
        IMapper mapper)
    : ControllerBase
{
    [Authorize(Roles = "Member")]
    [HttpPost] // api/review
    public async Task<IActionResult> CreateReview([FromBody] ReviewDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var review = mapper.Map<Review>(dto);
        review.UserId = userId;
        review.CreatedAt = DateTime.Now;

        var result = await service.AddReview(review);
        var response = mapper.Map<ReviewDto>(result);
        return Ok(response);
    }

    [Authorize(Roles = "Member")]
    [HttpPost("{reviewId:long}")] // api/review/{reviewId}?isLike={isLike}
    public async Task<IActionResult> LikeReview([FromRoute] long reviewId, [FromQuery] bool isLike)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        await service.RateReview(reviewId, userId, isLike);
        return Ok(isLike);
    }
}