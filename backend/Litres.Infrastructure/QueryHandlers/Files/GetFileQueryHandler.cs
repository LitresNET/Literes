using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Files;

public class GetFileQueryHandler(
    IConfiguration configuration,
    IAmazonS3 s3Client
    ) : IQueryHandler<GetFile, Stream>
{
    private readonly string _tempBucketName = configuration["AWS:TempBucketName"] ??
                                              throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");
    private readonly string _permBucketName = configuration["AWS:PermanentBucketName"] ??
                                              throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");
    
    public async Task<Stream> HandleAsync(GetFile q)
    {
        var s3Object = await s3Client.GetObjectAsync(_permBucketName, q.FileName) 
                       ?? await s3Client.GetObjectAsync(_tempBucketName, q.FileName);
        return s3Object.ResponseStream;
    }
}