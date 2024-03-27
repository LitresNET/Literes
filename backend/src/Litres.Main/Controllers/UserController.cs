using AutoMapper;
using backend.Exceptions;
using Litres.Data.Dto.Requests;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class UserController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("api/[controller]/signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
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