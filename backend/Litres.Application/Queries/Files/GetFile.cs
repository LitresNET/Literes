using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Queries.Files;

public class GetFile(long userId, string fileName) : IQuery<IFormFile?>
{
    public long UserId { get; set; } = userId;
    public string FileName { get; set; } = fileName;
}