using System.Security.Claims;
using AutoMapper;
using backend.Abstractions;
using backend.Dto.Requests;
using backend.Exceptions;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
public class BookController(IBookService bookService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("/publish")]
    public async Task<ActionResult<Request>> PublishBook(
        [FromBody] BookCreateRequestDto bookDto,
        [FromServices] IAuthorRepository authorRepository,
        [FromServices] ISeriesRepository seriesRepository
    )
    {
        try
        {
            var book = mapper.Map<Book>(bookDto);
            var result = await bookService.PublishNewBookAsync(book, authorRepository, seriesRepository);
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
}