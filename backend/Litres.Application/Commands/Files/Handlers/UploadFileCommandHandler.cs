using Amazon.S3;
using Amazon.S3.Model;
using Litres.Domain.Abstractions.Commands;
using Microsoft.Extensions.Configuration;

namespace Litres.Application.Commands.Files.Handlers;

public class UploadFileCommandHandler(
    IAmazonS3 s3Client, 
    IConfiguration configuration
    ) : ICommandHandler<UploadFileCommand>
{
    private readonly string _bucketName = configuration["AWS:BucketName"]!;
    
    public async Task HandleAsync(UploadFileCommand command)
    {
        var file = command.File;
        var fileName = command.UserId + ':' + file.FileName + ':' + Guid.NewGuid();
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            ContentType = file.ContentType
        };

        var initiateResponse = await s3Client.InitiateMultipartUploadAsync(initiateRequest);
        var uploadId = initiateResponse.UploadId;

        var partResponses = new List<PartETag>();
        
        const int partSize = 5 * 1024 * 1024; // 5 MB
        await using (var fileStream = file.OpenReadStream())
        {
            for (var i = 0; fileStream.Position < fileStream.Length; i++)
            {
                var buffer = new byte[partSize];
                var bytesRead = await fileStream.ReadAsync(buffer);
                if (bytesRead == 0) break;

                using var stream = new MemoryStream(buffer, 0, bytesRead);
                var uploadPartRequest = new UploadPartRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    UploadId = uploadId,
                    PartNumber = i + 1,
                    PartSize = bytesRead,
                    InputStream = stream
                };

                var uploadPartResponse = await s3Client.UploadPartAsync(uploadPartRequest);
                partResponses.Add(new PartETag(i+1, uploadPartResponse.ETag));
            }
        }
        
        var completeRequest = new CompleteMultipartUploadRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            UploadId = uploadId,
            PartETags = partResponses
        };
        await s3Client.CompleteMultipartUploadAsync(completeRequest);
    }
}