using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("user")]
    public async Task<IActionResult> SignUpUser([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("publisher")]
    public async Task<IActionResult> SignUpPublisher([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("user/final")]
    public async Task<IActionResult> FinalizeUser([FromBody] UserRegistrationDto dto)
    {
        var tenant = mapper.Map<User>(dto);
        var result = await registrationService.FinalizeUserAsync(tenant);
        return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
    }
}