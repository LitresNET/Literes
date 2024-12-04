using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Commands.Reviews;
using Litres.Application.Dto;
using Litres.Application.Models;
using Litres.Application.Queries.Reviews;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[Authorize(Roles = "Member")]
[ApiController]
[Route("api/[controller]")]
public class ReviewController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher
    ) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{reviewId:long}")] // api/review/{reviewId}
    public async Task<IActionResult> GetReview([FromRoute] long reviewId)
    {
        var query = new GetReview(reviewId);
        var r = await queryDispatcher.QueryAsync<GetReview, ReviewDto>(query);
        return Ok(r);
    }
    
    [AllowAnonymous]
    [HttpGet("list")] // api/review/list?bookId={bookId}&n={page}
    public async Task<IActionResult> GetReviewList([FromQuery] long bookId, [FromQuery] int page)
    {
        var query = new GetReviewList(bookId, page);
        var r = await queryDispatcher.QueryAsync<GetReviewList, List<ReviewDto>>(query);
        return Ok(r);
    }

    [HttpPost] // api/review
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        command.UserId = userId;
        command.CreatedAt = DateTime.Now;
        
        var response = await commandDispatcher.DispatchReturnAsync<CreateReviewCommand, ReviewDto>(command);
        return Ok(response);
    }

    [HttpPost("{reviewId:long}/rate")] // api/review/{reviewId}/rate?isLike={isLike}
    public async Task<IActionResult> RateReview([FromRoute] long reviewId, [FromQuery] bool isLike)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var command = new RateReviewCommand { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        var response = await commandDispatcher.DispatchReturnAsync<RateReviewCommand, bool>(command);
        return Ok(response);
    }

    [NonAction] // не работает по причине того, что нужно разорвать связь m-t-m, я пока не разобрался как это делать
    [HttpPost("{reviewId:long}/rate/remove")]
    public async Task<IActionResult> RemoveReviewRate([FromRoute] long reviewId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var command = new RemoveReviewRateCommand(reviewId, userId);
        await commandDispatcher.DispatchAsync(command);
        return Ok();
    }
}