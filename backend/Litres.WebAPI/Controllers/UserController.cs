using System.Globalization;
using System.Security.Claims;
using Litres.Application.Commands.Users;
using Litres.Application.Dto;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{UserId:long}")] // api/user/{userId}
    public async Task<IActionResult> GetUserPublicData([FromRoute] GetUserPublicData query)
    {
        var result = await queryDispatcher.QueryAsync<GetUserPublicData, UserPublicDataDto>(query);
        return Ok(result);
    }
    
    [HttpGet("order/list")] // api/user/order/list
    public async Task<IActionResult> GetOrderList()
    {
        //TODO: чет придумать с userId 
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        var result = await queryDispatcher.QueryAsync<GetOrderList, IEnumerable<OrderDto>>(new GetOrderList(userId));
        return Ok(result);
    }

    [HttpGet("settings")] // api/user/settings
    public async Task<IActionResult> GetUserPrivateData()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        var result = await queryDispatcher.QueryAsync<GetUserPrivateData, UserPrivateDataDto>(
            new GetUserPrivateData(userId));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{PublisherId:long}")] // api/user/publisher/{publisherId}
    public async Task<IActionResult> GetPublisherData([FromRoute] GetPublisherData query)
    {
        var result = await queryDispatcher.QueryAsync<GetPublisherData, PublisherStatisticsDto>(query);
        return Ok(result);
    }

    [HttpPost("deposit")] // api/user/deposit?amount={amount}
    public async Task<IActionResult> DepositToUser([FromQuery] decimal amount)
    {
        //TODO:
        // var securityToken = HttpContext.Request.Headers["X-Payment-Security-Token"].ToString();
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        await commandDispatcher.DispatchAsync(new DepositToUserCommand(userId, amount));
        return Ok();
    }

    [HttpPatch("settings")] // api/user/settings
    public async Task<IActionResult> ChangeUserData([FromBody] ChangeUserDataCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        command.UserId = userId;
        
        var response = await commandDispatcher.DispatchReturnAsync<ChangeUserDataCommand, UserSettingsDto>(command);
        return Ok(response);
    }
    /* 
    [HttpDelete("favourite/{bookId:long}")] // api/user/favourite/{bookId}
    public async Task<IActionResult> DeleteBookFromUserFavourites(long bookId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        await service.DeleteBookFromFavouritesAsync(userId, bookId);
        return Ok();
    }
    */ //Сделал пока один метод, т.к. на фронте сложнее реализовать функционал удаления книги из избранного
    [HttpPost("favourite/{BookId:long}")] // api/user/favourite/{bookId}
    public async Task<IActionResult> AddOrDeleteBookToUserFavourites([FromRoute] AddOrDeleteBookToUserFavouritesCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        command.UserId = userId;
        
        await commandDispatcher.DispatchAsync(command);
        return Ok();
    }
}   
