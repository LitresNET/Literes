using System.Security.Claims;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[Route("api/signin")]
public class SigninController(ILoginService loginService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SigninUserAsync([FromBody] UserLoginDto loginDto)
    {
        var token = await loginService.LoginUserAsync(loginDto.Email, loginDto.Password);
        return Ok(token);
    }
    
    [HttpGet("google")]
    public async Task<IActionResult> SigninWithGoogle()
    { 
        var authenticationProperties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("callback/google")]
    public async Task<IActionResult> GoogleResponseAsync()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return BadRequest();
        
        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        
        var token = await loginService.LoginUserFromExternalServiceAsync(email!, authenticateResult.Principal.Claims);
        return Ok(token);
    }
}