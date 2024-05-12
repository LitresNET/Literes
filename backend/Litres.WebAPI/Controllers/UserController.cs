using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController(
    IUserService service,
    IMapper mapper) 
    : ControllerBase
{
    [HttpGet("{userId:long}")] // api/user/{userId}
    public async Task<IActionResult> GetUserData(long userId)
    {
        var user = await service.GetSafeUserInfoAsync(userId);
        var result = mapper.Map<UserSafeDataDto>(user);
        return Ok(result);
    }
    
    [HttpGet("order/list")]
    public async Task<IActionResult> GetOrderList()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var result = await service.GetOrderListAsync(userId);
        var response = result.Select(mapper.Map<OrderDto>);
        return Ok(response);
    }

    [HttpGet("settings")] // api/user/settings
    public async Task<IActionResult> GetPrivateUserData()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var user = await service.GetUserByIdAsync(userId);
        var result = mapper.Map<UserDataDto>(user);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{publisherId:long}")] // api/user/publisher/{publisherId}
    public async Task<IActionResult> GetPublisherData(long publisherId)
    {
        var publisher = await service.GetPublisherInfoAsync(publisherId);
        var result = mapper.Map<PublisherStatisticsDto>(publisher);
        return Ok(result);
    }

    [HttpPost("deposit")] // api/user/deposit?amount={amount}
    public async Task<IActionResult> DepositToUser([FromQuery] decimal amount)
    {
        // var securityToken = HttpContext.Request.Headers["X-Payment-Security-Token"].ToString();
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        await service.DepositToUserByIdAsync(userId, amount);
        return Ok();
    }

    [HttpPatch("settings")] // api/user/settings
    public async Task<IActionResult> ChangeUserData([FromBody] UserSettingsDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var user = mapper.Map<User>(dto);
        user.Id = userId;
        var resultUser = await service.ChangeUserSettingsAsync(user);
        var response = mapper.Map<UserSettingsDto>(resultUser);
        return Ok(response);
    }

    [HttpDelete("favourite/{bookId:long}")] // api/user/favourite/{bookId}
    public async Task<IActionResult> DeleteBookFromUsersFavourites(long bookId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var result = await service.UnFavouriteBookAsync(userId, bookId);
        return Ok(result);
    }
}   
