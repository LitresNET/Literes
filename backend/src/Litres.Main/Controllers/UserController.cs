using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper, IConfiguration configuration) : ControllerBase
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
    
    /*
    [HttpGet("sign-google")]
    public IActionResult Authorize()
    {
        var authUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
                      "client_id=" + configuration["Authentication:Google:ClientId"] +
                      "&redirect_uri=" + configuration["Authentication:Google:RedirectUri"] +
                      "&response_type=code" +
                      "&scope=openid%20email%20profile";
        
        return Redirect(authUrl);
    }
    */
    
    [HttpGet("signin-google")]
    public async Task<IActionResult> SignInWithGoogle()
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = Url.Action(configuration["Authentication:Google:RedirectUri"]) };
        
        //return Challenge(authenticationProperties, JwtBearerDefaults.AuthenticationScheme);
        throw new NotImplementedException();
    }
    
    [HttpGet("callback-google")]
    public async Task<IActionResult> LoginGoogleAsync()
    {
        throw new NotImplementedException();
    }
}