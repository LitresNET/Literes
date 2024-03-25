using System.ComponentModel.DataAnnotations;
using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Dto.Responses;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers;

[ApiController]
public class RegistrationController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost("api/[controller]/signup")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        var result = await registrationService.RegisterUserAsync(user);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
}