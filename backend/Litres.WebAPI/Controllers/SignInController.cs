using System.Security.Claims;
using Litres.Application.Commands.SignIn;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignInController(ICommandDispatcher commandDispatcher, ILoginService loginService) : ControllerBase
{
    [HttpPost] // api/signin
    public async Task<IActionResult> SignInUser([FromBody] SignInUserCommand command)
    {
        var token = await commandDispatcher.DispatchReturnAsync<SignInUserCommand, string>(command);
        return Ok(token);
    }
    
    [HttpGet("google")] // api/signin/google
    public Task<IActionResult> SignInWithGoogle()
    { 
        var authenticationProperties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        return Task.FromResult<IActionResult>(Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme));
    }
    
    //TODO: Переписать под CQRS
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