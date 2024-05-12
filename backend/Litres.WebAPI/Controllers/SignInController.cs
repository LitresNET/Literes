using System.Security.Claims;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignInController(ILoginService loginService) : ControllerBase
{
    [HttpPost] // api/signin
    public async Task<IActionResult> SignInUser([FromBody] UserLoginDto loginDto)
    {
        var token = await loginService.LoginUserAsync(loginDto.Email, loginDto.Password);
        return Ok(token);
    }
    
    [HttpGet("google")] // api/signin/google
    public async Task<IActionResult> SignInWithGoogle()
    { 
        var authenticationProperties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("callback-google")] // api/signin/callback-google
    public async Task<IActionResult> GoogleResponse()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return BadRequest();
        
        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var token = await loginService.LoginUserFromExternalServiceAsync(email!, authenticateResult.Principal.Claims);
        return token is "" ? BadRequest() : Ok(token);
    }
}