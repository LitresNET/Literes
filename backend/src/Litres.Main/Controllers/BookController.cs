using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService, IMapper mapper) : ControllerBase
{
    [HttpPost("publish")]
    public async Task<ActionResult<Request>> PublishBook([FromBody] BookCreateRequestDto bookDto)
    {
        var book = mapper.Map<Book>(bookDto);
        var result = await bookService.PublishNewBookAsync(book);
        var answer = mapper.Map<BookCreateResponseDto>(result);
        return Ok(answer);
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteBook([FromRoute] long id, [FromQuery] long publisherId)
    {
        var result = await bookService.DeleteBookAsync(id, publisherId);
        return Ok(result);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateBook([FromRoute] long id, [FromBody] BookUpdateRequestDto bookDto, [FromQuery] long publisherId)
    {
        var book = mapper.Map<Book>(bookDto);
        var result = await bookService.UpdateBookAsync(book, publisherId);
        return Ok(result);
    }
}
