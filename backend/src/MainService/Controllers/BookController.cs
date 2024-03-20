using System.Security.Claims;
using backend.Abstractions;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpPost]
    [Route("/publish")]
    public async Task<IActionResult> PublishBook([FromBody] Book book, ClaimsPrincipal publisher)
    {
        var publisherIdClaim = publisher.Claims.Single(claim => claim.Type == CustomClaimTypes.UserId);
        if (!long.TryParse(publisherIdClaim.Value, out var publisherId))
            return BadRequest("Publisher ID is not an integer number");
        
        book.PublisherId = publisherId;
        var result = await bookService.PublishNewBookAsync(book);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}/delete")]
    public async Task<IActionResult> DeleteBook([FromRoute] long id, ClaimsPrincipal publisher)
    {
        var publisherIdClaim = publisher.Claims.Single(claim => claim.Type == CustomClaimTypes.UserId);
        if (!long.TryParse(publisherIdClaim.Value, out var publisherId))
            return BadRequest("Publisher ID is not an integer number");
        
        var result = await bookService.DeleteBookAsync(id, publisherId);
        return Ok(result);
    }
}