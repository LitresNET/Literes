using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Files;

public class GetFileQueryHandler(
    IFileService fileService
    ) : IQueryHandler<GetFile, (Stream stream, string contentType, string fileName)>
{

    public async Task<(Stream stream, string contentType, string fileName)> HandleAsync(GetFile q)
    {
        return await fileService.GetFileAsync(q.FileName);
    }
}