using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper, SignInManager<User> signInManager): ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await userService.RegisterUserAsync(user);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("signup/publisher")]
    public async Task<IActionResult> RegisterPublisherAsync([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await userService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDto loginDto)
    {
        var token = await userService.LoginUserAsync(loginDto.Email, loginDto.Password);
        return Ok(token);
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
        return Ok();
    }
}