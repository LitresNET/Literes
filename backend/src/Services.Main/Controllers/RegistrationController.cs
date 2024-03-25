using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class RegistrationController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("api/[controller]/signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
}