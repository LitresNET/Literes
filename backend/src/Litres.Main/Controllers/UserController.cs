using System.Security.Claims;
using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
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

    [HttpGet("user/get-data/{userId:long}")]
    public async Task<IActionResult> GetSafeUserData([FromQuery] long userId)
    {
        var user = await userService.GetSafeUserDataAsync(userId);
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
        var result = await userService.GetUserDataAsync(userId);
        return Ok(result);
    }
    
    [HttpGet("publisher/get-data/{publisherId:long}")]
    public async Task<IActionResult> GetPublisherData([FromQuery] long publisherId)
    {
        var publisher = await userService.GetPublisherAsync(publisherId);
        var result = mapper.Map<PublisherStatisticsDto>(publisher);
        return Ok(result);
    }
  
}
        
    [HttpGet("signin-google")]
    public IActionResult SignInWithGoogle()
    { 
        var authenticationProperties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("callback-google")]
    public async Task<IActionResult> GoogleResponseAsync()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return BadRequest();
        }
        
        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        
        var token = await registrationService.LoginUserFromExternalServiceAsync(email!, authenticateResult.Principal.Claims);
        return Ok(token);
    }
    
    [HttpGet("test")]
    [Authorize(Roles = "Publisher")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}   
