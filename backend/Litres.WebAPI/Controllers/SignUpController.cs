using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("user")] // api/signup/user
    public async Task<IActionResult> SignUpUser([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("agent")] // api/signup/agent
    public async Task<IActionResult> SignUpAgent([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserWithRoleAsync(user, "Agent");
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("publisher")] // api/signup/publisher
    public async Task<IActionResult> SignUpPublisher([FromBody] PublisherRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterPublisherAsync(user, registrationDto.ContractNumber);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
    
    [HttpPost("user/final")] // api/signup/user/final
    public async Task<IActionResult> FinalizeUser([FromBody] UserRegistrationDto dto)
    {
        var user = mapper.Map<User>(dto);
        var result = await registrationService.FinalizeUserAsync(user);
        return result.Succeeded 
            ? Ok() 
            : BadRequest(result.Errors.Select(e => e.Description));
    }
}