using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Exceptions;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

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
        catch (BookValidationFailedException e)
        {
            return UnprocessableEntity(e);
        }
        catch (AuthorNotFoundException e)
        {
            return NotFound(e);
        }
        catch (SeriesNotFoundException e)
        {
            return NotFound(e);
        }
    }

    [HttpDelete]
    [Route("{id}/delete")]
    public async Task<IActionResult> DeleteBook([FromRoute] long id, [FromQuery] long publisherId)
    {
        try
        {
            var result = await bookService.DeleteBookAsync(id, publisherId);
            return Ok(result);
        }
        catch (BookNotFoundException e)
        {
            return NotFound(e);
        }
        catch (UserPermissionDeniedException e)
        {
            return Forbid(e.Message);
        }
    }
    
    [HttpPatch]
    [Route("{id}/update")]
    public async Task<IActionResult> UpdateBook([FromBody] BookUpdateRequestDto bookDto, [FromQuery] long publisherId)
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var result = await bookService.UpdateBookAsync(book, publisherId);
            return Ok(result);
        }
        catch (BookNotFoundException e)
        {
            return NotFound(e);
        }
        catch (UserPermissionDeniedException e)
        {
            return Forbid(e.Message);
        }
    }
}
