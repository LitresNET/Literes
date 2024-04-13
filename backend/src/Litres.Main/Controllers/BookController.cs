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
        var request = await bookService.PublishNewBookAsync(book);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("delete/{bookId:long}")]
    public async Task<IActionResult> DeleteBook([FromRoute] long bookId, [FromQuery] long publisherId)
    {
        await bookService.DeleteBookAsync(bookId, publisherId);
        return Ok();
    }
    
    [HttpPatch("update/{bookId:long}")]
    public async Task<IActionResult> UpdateBook([FromRoute] long bookId, [FromBody] BookUpdateRequestDto bookDto, [FromQuery] long publisherId)
    {
        var book = mapper.Map<Book>(bookDto);
        book.Id = bookId;
        var request = await bookService.UpdateBookAsync(book, publisherId);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }
}
