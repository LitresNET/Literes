using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Queries.Files;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Files;

public class GetFileQueryHandler(
    IConfiguration config,
    ApplicationDbContext dbContext,
    IAmazonS3 s3Client
    ) : IQueryHandler<GetFile, IFormFile?>
{
    public async Task<IFormFile?> HandleAsync(GetFile q)
    {
        var userId = q.UserId;
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return null;

        if (!int.TryParse(q.FileName.Split(':')[0], out var fileUserId))
            return null;

        if (userId != fileUserId)
            return null;

        var bucketName = config["AWS:BucketName"];
        if (bucketName == null)
            throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");
        
        var s3Object = await s3Client.GetObjectAsync(bucketName, q.FileName);

        return new FormFile(s3Object.ResponseStream, 0, s3Object.ResponseStream.Length, 
            q.FileName, q.FileName);
    }
}