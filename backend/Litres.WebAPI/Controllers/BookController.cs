using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Commands.Books;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Application.Queries.Books;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/book
public class BookController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{bookId:long}")] // api/book/{bookId}
    public async Task<IActionResult> GetBook([FromRoute] GetBook query)
    {
        var result = await queryDispatcher.QueryAsync<GetBook, BookResponseDto>(query);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("catalog/{pageNumber:int}/{amount:int}")] // api/book/catalog/{pageNumber}/{amount}
    public async Task<IActionResult> GetBookCatalog([FromRoute][FromQuery] GetBookCatalog query)
    {
        var result = await queryDispatcher.QueryAsync<GetBookCatalog, List<BookResponseDto>>(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPost] // api/book
    public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        command.Book.AuthorId = userId;
        var request = await commandDispatcher.DispatchReturnAsync<CreateBookCommand, RequestResponseDto>(command);
        return Ok(request);
    }
    
    [Authorize(Roles = "Publisher")]
    [HttpPatch("{bookId:long}")]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] GetBook query,
        [FromRoute][FromQuery][FromBody] UpdateBookCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var dbBook = await queryDispatcher.QueryAsync<GetBook, BookResponseDto>(query);
        if (dbBook.AuthorId != userId)
            return Forbid();
        
        var request = await commandDispatcher.DispatchReturnAsync<UpdateBookCommand, RequestResponseDto>(command);
        return Ok(request);
    }

    [Authorize(Roles = "Publisher")]
    [HttpDelete("{bookId:long}")] // api/book/{bookId}
    public async Task<IActionResult> DeleteBook([FromRoute] GetBook query, [FromRoute][FromQuery] DeleteBookCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var book = await queryDispatcher.QueryAsync<GetBook, BookResponseDto>(query);
        if (book.AuthorId != userId)
            return Forbid();
        
        await commandDispatcher.DispatchAsync(command);
        return Ok();
    }
}
