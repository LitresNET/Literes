using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Queries.Files;

public record GetFile(string FileName) : IQuery<(Stream stream, string contentType, string fileName)>
{
}