using System.Globalization;
using System.Security.Claims;
using AutoMapper;
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
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpGet("{userId:long}")] // api/user/{userId}
    public async Task<IActionResult> GetUserData(long userId)
    {
        var user = await userService.GetSafeUserInfoAsync(userId);
        var result = mapper.Map<UserSafeDataDto>(user);
        return Ok(result);
    }

    [HttpGet("settings")] // api/user/settings
    public async Task<IActionResult> GetPrivateUserData()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var user = await userService.GetUserInfoAsync(userId);
        var result = mapper.Map<UserDataDto>(user);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{publisherId:long}")] // api/user/publisher/{publisherId}
    public async Task<IActionResult> GetPublisherData(long publisherId)
    {
        var publisher = await userService.GetPublisherInfoAsync(publisherId);
        var result = mapper.Map<PublisherStatisticsDto>(publisher);
        return Ok(result);
    }

    [HttpPatch("settings")] // api/user/settings
    public async Task<IActionResult> ChangeUserData([FromBody] UserSettingsDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var user = mapper.Map<User>(dto);
        user.Id = userId;
        var resultUser = await userService.ChangeUserSettingsAsync(user);
        var response = mapper.Map<UserSettingsDto>(resultUser);
        return Ok(response);
    }

    [HttpDelete("favourite/{bookId:long}")] // api/user/favourite/{bookId}
    public async Task<IActionResult> DeleteBookFromUsersFavourites(long bookId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var result = await userService.UnFavouriteBookAsync(userId, bookId);
        return Ok(result);
    }

    [HttpGet("test")]
    [Authorize(Roles = "Publisher")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}   
