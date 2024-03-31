using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
public class BookController(IBookService bookService, IMapper mapper) : ControllerBase
{
    [HttpPost("api/[controller]/publish")]
    public async Task<ActionResult<Request>> PublishBook([FromBody] BookCreateRequestDto bookDto)
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var result = await bookService.PublishNewBookAsync(book);
            return Ok(result);
        }
        catch (EntityValidationFailedException<Book> e)
        {
            return UnprocessableEntity(e.Message);
        }
        catch (EntityNotFoundException<Book> e)
        {
            return NotFound(e);
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("{id}/delete")]
    public async Task<IActionResult> DeleteBook([FromRoute] long id, [FromQuery] long publisherId)
    {
        try
        {
            var result = await bookService.DeleteBookAsync(id, publisherId);
            return Ok(result);
        }
        catch (EntityNotFoundException<Book> e)
        {
            return NotFound(e);
        }
        catch (PermissionDeniedException e)
        {
            return Forbid();
        }
    }
    
    [HttpPatch]
    [Route("{id}/update")]
    public async Task<IActionResult> UpdateBook(long id, [FromBody] BookUpdateRequestDto bookDto, [FromQuery] long publisherId)
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var result = await bookService.UpdateBookAsync(book, publisherId);
            return Ok(result);
        }
        catch (EntityNotFoundException<Book> e)
        {
            return NotFound(e);
        }
        catch (PermissionDeniedException e)
        {
            return Forbid(e.Message);
        }
    }
}
