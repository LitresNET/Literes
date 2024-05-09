using System.Globalization;
using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return BadRequest(); //TODO: везде поменять проверку claim на единую и изменить BadRequest на Unauthorized

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

    [Authorize]
    [HttpPatch("{reviewId:long}/change")]
    public async Task<IActionResult> ChangeReview([FromRoute] long reviewId, [FromBody] ReviewChangeRequestDto reviewDto)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        var dbReview = await reviewService.GetReviewInfo(reviewId);
        if (userId != dbReview.UserId)
            return Forbid();
        dbReview.Content = reviewDto.Content;
        if (dbReview.BookId is not null)
            dbReview.Rating = reviewDto.Rating;
        await reviewService.UpdateReview(dbReview);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPatch("admin/{reviewId:long}/change")]
    public async Task<IActionResult> AdminChangeReview([FromRoute] long reviewId, [FromBody] ReviewChangeRequestDto reviewDto)
    {
        var dbReview = await reviewService.GetReviewInfo(reviewId);
        dbReview.Content = reviewDto.Content;
        if (dbReview.BookId is not null)
            dbReview.Rating = reviewDto.Rating;
        await reviewService.UpdateReview(dbReview);
        return Ok();
    }
    
    [Authorize]
    [HttpPatch("{reviewId:long}/delete")]
    public async Task<IActionResult> DeleteReview([FromRoute] long reviewId)
    {
        //TODO: атрибут Authorize уже предполагает, что пользователь авторизован. Мне кажется эта проверка лишняя
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        var dbReview = await reviewService.GetReviewInfo(reviewId);
        if (userId != dbReview.UserId)
            return Forbid();
        await reviewService.DeleteReview(dbReview);
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPatch("admin/{reviewId:long}/delete")]
    public async Task<IActionResult> AdminDeleteReview([FromRoute] long reviewId)
    {
        await reviewService.DeleteReview(reviewId);
        return Ok();
    }
}