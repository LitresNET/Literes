using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/signup")]
public class SignUpController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("user")]
    public async Task<IActionResult> SignUpUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("publisher")]
    public async Task<IActionResult> SignUpPublisherAsync([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}