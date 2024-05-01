using System.Globalization;
using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewService reviewService, IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpPost("post")]
    public async Task<IActionResult> PostReview([FromBody] ReviewCreateRequestDto dto)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        var review = mapper.Map<Review>(dto);
        review.UserId = userId;
        review.CreatedAt = DateTime.Now;

        await reviewService.AddReview(review);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("{reviewId:long}/like")]
    public async Task<IActionResult> LikeReview([FromRoute] long reviewId)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        await reviewService.LikeReview(reviewId, userId);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("{reviewId:long}/dislike")]
    public async Task<IActionResult> DislikeReview([FromRoute] long reviewId)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        await reviewService.DislikeReview(reviewId, userId);
        return Ok();
    }

    [HttpGet("{bookId:long}/getbybook")]
    public async Task<IActionResult> GetByBookId([FromRoute] long bookId)
    {
        var reviewList = await reviewService.GetByBookAsync(bookId);
        var result = mapper.Map<List<ReviewResponseDto>>(reviewList);
        return Ok(result);
    }
    
    [HttpGet("{parentReviewId:long}/getbybook")]
    public async Task<IActionResult> GetByParentReview([FromRoute] long parentReviewId)
    {
        var reviewList =  await reviewService.GetByParentReviewAsync(parentReviewId);
        var result = mapper.Map<List<ReviewResponseDto>>(reviewList);
        return Ok(result);
    }
}