using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Files;

public class GetFileQueryHandler(
    IConfiguration config,
    IAmazonS3 s3Client
    ) : IQueryHandler<GetFile, Stream>
{
    public async Task<Stream> HandleAsync(GetFile q)
    {
        var bucketName = config["AWS:BucketName"];
        if (bucketName == null)
            throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");
        
        var s3Object = await s3Client.GetObjectAsync(bucketName, q.FileName.Split('.')[1]);
        return s3Object.ResponseStream;
    }
}