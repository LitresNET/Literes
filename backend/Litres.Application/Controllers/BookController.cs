using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")] // api/book
public class BookController(
    IBookService service, 
    IMapper mapper) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{bookId:long}")] // api/book/{bookId}
    public async Task<IActionResult> GetBook(long bookId)
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var result = await service.GetBookInfoAsync(bookId);
        var response = mapper.Map<BookResponseDto>(result);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("catalog/{pageNumber:int}/{amount:int}")] // api/book/catalog/{pageNumber}/{amount}
    public async Task<IActionResult> GetBookCatalog(
        [FromRoute] int pageNumber,
        [FromRoute] int amount,
        [FromQuery] Dictionary<SearchParameterType, string> searchParameters)
    {
        var result = await service.GetBookCatalogAsync(searchParameters, pageNumber, amount);
        var response = mapper.Map<List<BookResponseDto>>(result);
        return Ok(response);
    }

    [Authorize(Roles = "Publisher")]
    [HttpPost] // api/book
    public async Task<IActionResult> CreateBook([FromBody] BookCreateRequestDto bookDto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        bookDto.AuthorId = userId;
        var book = mapper.Map<Book>(bookDto);
        var request = await service.CreateBookAsync(book);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }
    
    [Authorize(Roles = "Publisher")]
    [HttpPatch("{bookId:long}")]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] long bookId,
        [FromQuery] long publisherId,
        [FromBody] BookUpdateRequestDto bookDto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var dbBook = await service.GetBookInfoAsync(bookId);
        if (dbBook.AuthorId != userId)
            return Forbid();
        
        var book = mapper.Map<Book>(bookDto);
        book.Id = bookId;
        var request = await service.UpdateBookAsync(book, publisherId);
        var result = mapper.Map<RequestResponseDto>(request);
        return Ok(result);
    }

    [Authorize(Roles = "Publisher")]
    [HttpDelete("{bookId:long}")] // api/book/{bookId}
    public async Task<IActionResult> DeleteBook([FromRoute] long bookId, [FromQuery] long publisherId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var book = await service.GetBookInfoAsync(bookId);
        if (book.AuthorId != userId)
            return Forbid();
        
        await service.DeleteBookAsync(bookId, publisherId);
        return Ok();
    }
}
