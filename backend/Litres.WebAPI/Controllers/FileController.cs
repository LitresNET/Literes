﻿using System.Globalization;
using System.Security.Claims;
using Litres.Application.Commands.Files;
using Litres.Application.Models;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/file
public class FileController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher
    ) : ControllerBase
{
    //TODO: Не работает
    [HttpGet("list")]
    public async Task<IActionResult> GetFiles()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);
        var query = new GetFiles(userId);
        var result = await queryDispatcher.QueryAsync<GetFiles, List<IFormFile>?>(query);
        return result is null ? Ok() : Ok(result);
    }

    [HttpGet("{FileName}")] // /api/file
    public async Task<IActionResult> GetFile([FromRoute] GetFile query)
    {
        //TODO: query должно возвращать dto со stream и корректным (без id) названием файла, чтобы в контроллере не было
        // преобразования имени через Split(':')
        var stream = await queryDispatcher.QueryAsync<GetFile, Stream>(query);
        return File(stream, "application/octet-stream", query.FileName.Split(':')[1]);
    }

    
    [HttpPost("upload")] // api/file/upload
    public async Task<IActionResult> UploadFile([FromForm] UploadFileToTempCommand command)
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);
        command.UserId = userId;
        
        var result = await commandDispatcher.DispatchReturnAsync<UploadFileToTempCommand, string>(command);
        return Ok(result);
    }
}