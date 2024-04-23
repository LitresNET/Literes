using System.Globalization;
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
    [Authorize]
    [HttpGet("read/{bookId:long}")]
    public async Task<IActionResult> Get(long bookId)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        var result = await bookService.GetBookWithAccessCheckAsync(userId, bookId);
        var response = mapper.Map<BookResponseDto>(result);
        return Ok(response);
    }
    
    [HttpGet("catalog/page/{pageNumber:int}/take/{amount:int}")]
    public async Task<IActionResult> GetCatalog(
        [FromQuery] Dictionary<SearchParameter, string> searchParameters,
        [FromRoute] int pageNumber,
        [FromRoute] int amount)
    {
        var result = await bookService.GetBookCatalogAsync(searchParameters, pageNumber, amount);
        var response = mapper.Map<List<BookResponseDto>>(result);
        return Ok(response);
    }
    
    [HttpPost("publish")]
    public async Task<IActionResult> Publish([FromBody] BookCreateRequestDto bookDto)
    {
        var book = mapper.Map<Book>(bookDto);
        var request = await bookService.PublishNewBookAsync(book);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("delete/{bookId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long bookId, [FromQuery] long publisherId)
    {
        await bookService.DeleteBookAsync(bookId, publisherId);
        return Ok();
    }
    
    [HttpPatch("update/{bookId:long}")]
    public async Task<IActionResult> Update([FromRoute] long bookId, [FromBody] BookUpdateRequestDto bookDto, [FromQuery] long publisherId)
    {
        var book = mapper.Map<Book>(bookDto);
        book.Id = bookId;
        var request = await bookService.UpdateBookAsync(book, publisherId);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }
}
