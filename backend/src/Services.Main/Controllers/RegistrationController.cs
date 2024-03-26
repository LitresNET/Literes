using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Exceptions;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class RegistrationController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("api/[controller]/signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        Console.WriteLine(registrationDto.Password);
        var user = mapper.Map<User>(registrationDto);
        Console.WriteLine(user.PasswordHash);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("api/[controller]/signup/publisher")]
    public async Task<IActionResult> RegisterPublisherAsync([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost("api/[controller]/signin")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserLoginDto loginDto)
    {
        try
        {
            var token = await registrationService.LoginUserAsync(loginDto.Email, loginDto.Password);
            return Ok(token);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (PasswordNotMatchException e)
        {
            return BadRequest(e.Message);
        }
    }
}