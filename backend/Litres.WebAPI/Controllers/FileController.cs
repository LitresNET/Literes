using Litres.Application.Commands.Files;
using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/file
public class FileController(ICommandDispatcher commandDispatcher) : ControllerBase
{
    [HttpPost("upload")] // api/file/upload
    public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand command)
    {
        await commandDispatcher.DispatchAsync(command);
        return Ok();
    }
    
}