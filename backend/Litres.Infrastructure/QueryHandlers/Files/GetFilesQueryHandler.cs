using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Files;

public class GetFilesQueryHandler(
    IAmazonS3 s3Client,
    ApplicationDbContext dbContext,
    IConfiguration config
    ) : IQueryHandler<GetFiles, List<IFormFile>?>
{
    public async Task<List<IFormFile>?> HandleAsync(GetFiles q)
    {
        var userId = q.UserId;
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return [];

        var bucketName = config["AWS:BucketName"];
        if (bucketName == null)
            throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");

        return await GetFilesFromBucketWithPrefixAsync(bucketName, userId.ToString());
    }

    private async Task<List<IFormFile>> GetFilesFromBucketWithPrefixAsync(string bucketName, string prefix)
    {
        var s3Objects = await ListFilesWithPrefixAsync(bucketName, prefix);
        
        List<IFormFile> files = [];
        foreach (var s3Object in s3Objects)
        {
            var obj = await s3Client.GetObjectAsync(bucketName, s3Object.Key);
            var stream = obj.ResponseStream;
            files.Add(new FormFile(stream, 0, stream.Length, s3Object.Key, s3Object.Key));
        }

        return files;
    }
    
    
    // todo: rewrite to paginated query
    private async Task<List<S3Object>> ListFilesWithPrefixAsync(string bucketName, string prefix)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = prefix
        };

        var response = await s3Client.ListObjectsV2Async(request);
        var files = new List<S3Object>(response.S3Objects);

        while ((bool)response.IsTruncated!)
        {
            request.ContinuationToken = response.NextContinuationToken;
            response = await s3Client.ListObjectsV2Async(request);
            files.AddRange(response.S3Objects);
        }

        return files;
    }
}