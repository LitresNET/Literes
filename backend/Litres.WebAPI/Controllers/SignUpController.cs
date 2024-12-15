using Litres.Application.Commands.SignUp;
using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(ICommandDispatcher commandDispatcher) : ControllerBase
{
    [HttpPost("user")] // api/signup/user
    public async Task<IActionResult> SignUpUser([FromBody] SignUpUserCommand command)
    {
        var result = await commandDispatcher.DispatchReturnAsync<SignUpUserCommand, IdentityResult>(command);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("agent")] // api/signup/agent
    public async Task<IActionResult> SignUpAgent([FromBody] SignUpUserCommand command)
    {
        command.Role = "Agent";
        var result = await commandDispatcher.DispatchReturnAsync<SignUpUserCommand, IdentityResult>(command);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("admin")] // api/signup/admin
    public async Task<IActionResult> SignUpAdmin([FromBody] SignUpUserCommand command)
    {
        command.Role = "Admin";
        var result = await commandDispatcher.DispatchReturnAsync<SignUpUserCommand, IdentityResult>(command);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    //TODO: rewrite with some more logic
    [HttpPost("publisher")] // api/signup/publisher
    public async Task<IActionResult> SignUpPublisher([FromBody] SignUpUserCommand command)
    {
        command.Role = "Publisher";
        var result = await commandDispatcher.DispatchReturnAsync<SignUpUserCommand, IdentityResult>(command);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("user/final")] // api/signup/user/final
    public async Task<IActionResult> FinalizeUser([FromBody] FinalizeUserCommand command)
    {
        var result = await commandDispatcher.DispatchReturnAsync<FinalizeUserCommand, IdentityResult>(command);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
}