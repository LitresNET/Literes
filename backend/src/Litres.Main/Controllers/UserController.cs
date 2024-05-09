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
public class UserController(
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpPatch("settings")]
    public async Task<IActionResult> ChangeUserSettings([FromBody] UserSettingsDto dto)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        
        var user = mapper.Map<User>(dto);
        user.Id = userId;
        var resultUser = await userService.ChangeUserSettingsAsync(user);
        var response = mapper.Map<UserSettingsDto>(resultUser);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("favourites/{bookIdToDelete:long}")]
    public async Task<IActionResult> DeleteBookFromUsersFavourites(long bookIdToDelete)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();

        var result = await userService.UnFavouriteBookAsync(userId, bookIdToDelete);
        return Ok(result);
    }

    [HttpGet("user/get-data/{userId:long}")]
    public async Task<IActionResult> GetSafeUserData(long userId)
    {
        var user = await userService.GetSafeUserInfoAsync(userId);
        var result = mapper.Map<UserSafeDataDto>(user);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("user/get-data")]
    public async Task<IActionResult> GetUserOwnData()
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        var user = await userService.GetUserInfoAsync(userId);
        var result = mapper.Map<UserDataDto>(user);
        return Ok(result);
    }
    
    [HttpGet("publisher/get-data/{publisherId:long}")]
    public async Task<IActionResult> GetPublisherData(long publisherId)
    {
        var publisher = await userService.GetPublisherInfoAsync(publisherId);
        var result = mapper.Map<PublisherStatisticsDto>(publisher);
        return Ok(result);
    }
    
    [HttpGet("test")]
    [Authorize(Roles = "Publisher")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}   
