using System.Security.Claims;
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
    IRegistrationService registrationService, 
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("signup/publisher")]
    public async Task<IActionResult> RegisterPublisherAsync([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDto loginDto)
    {
        var token = await registrationService.LoginUserAsync(loginDto.Email, loginDto.Password);
        return Ok(token);
    }

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
    public async Task<IActionResult> DeleteBookFromUsersFavourites([FromQuery] long bookIdToDelete)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();

        var result = await userService.UnFavouriteBookAsync(userId, bookIdToDelete);
        return Ok(result);
    }

    [HttpGet("user/get_data/{userId:long}")]
    public async Task<IActionResult> GetSafeUserData([FromQuery] long userId)
    {
        var user = await userService.GetSafeUserData(userId);
        var result = mapper.Map<UserSafeDataDto>(user);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("user/get_data")]
    public async Task<IActionResult> GetUserOwnData()
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        var result = await userService.GetUserData(userId);
        return Ok(result);

    }
  
}