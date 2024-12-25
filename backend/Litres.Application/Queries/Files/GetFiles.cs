using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Queries.Files;

public class GetFiles(long userId) : IQuery<List<IFormFile>?>
{
    public long UserId { get; set; } = userId;
}