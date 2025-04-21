using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Files;

public record GetFile(string FileName) : IQuery<(Stream stream, string contentType, string fileName)>
{
}