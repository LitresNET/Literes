using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
public class BookController : ControllerBase
{
    [HttpPost]
    public IActionResult PublishBook([FromBody] Book book)
    {
        throw new NotImplementedException();
    }
    
    [HttpPatch]
    public IActionResult UpdateBook([FromBody] Book book)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    public IActionResult DeleteBook([FromQuery] int bookId)
    {
        throw new NotImplementedException();
    }
}