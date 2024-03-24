using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Dto.Responses;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
public class RegistrationController(IRegistrationService registrationService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("/signup")]
    public async Task<RegisterUserResponseDto> RegisterUserAsync([FromBody] UserRegistrationDto registrationDto)
    {
        throw new NotImplementedException();
    }
    
}