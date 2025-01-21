using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using File = TagLib.File;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Litres.Application.Services;

public class FileService(
    IAmazonS3 s3Client,
    IRedisRepository redisRepository, 
    IConfiguration configuration
    ) : IFileService
{
    private readonly string _permBucketName = configuration["AWS:PermanentBucketName"] ??
                                              throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");
    private readonly string _tempBucketName = configuration["AWS:TempBucketName"] ??
                                              throw new NullReferenceException("BucketName cannot be null. Configuration was incorrect.");

    public async Task<string> UploadFileToTempAsync(IFormFile file, long userId)
    {
        var fileName = userId.ToString() + ':' + file.FileName + ':' + Guid.NewGuid();
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = _tempBucketName,
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
                    BucketName = _tempBucketName,
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
            BucketName = _tempBucketName,
            Key = fileName,
            UploadId = uploadId,
            PartETags = partResponses
        };
        await s3Client.CompleteMultipartUploadAsync(completeRequest);
        var metadata = GetMetadataJson(file);
        await SaveMetadataAsync(fileName, metadata);

        return fileName;
    }

    public async Task<string?> UploadFileToPermAsync(string fileName)
    {
        var copyRequest = new CopyObjectRequest
        {
            SourceBucket = _tempBucketName,
            SourceKey = fileName,
            DestinationBucket = _permBucketName,
            DestinationKey = fileName,
            MetadataDirective = S3MetadataDirective.REPLACE
        };

        var metadataJson = await redisRepository.GetValue<string>(fileName);
        if (metadataJson is null)
        {
            return null;
        }
        if (metadataJson != string.Empty)
        {
            var metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(metadataJson);
            if (metadata != null)
            {
                foreach (var kvp in metadata)
                {
                    copyRequest.Metadata.Add(kvp.Key, kvp.Value);
                }
            }
        }
        await s3Client.CopyObjectAsync(copyRequest);
        
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _tempBucketName,
            Key = fileName
        };

        await s3Client.DeleteObjectAsync(deleteRequest);
        return fileName;
    }

    public async Task UploadAllFilesToPermAsync()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _tempBucketName
        };

        ListObjectsV2Response response;

        do
        {
            response = await s3Client.ListObjectsV2Async(request);
                
            foreach (var obj in response.S3Objects)
            {
                await UploadFileToPermAsync(obj.Key);
            }
                
            request.ContinuationToken = response.NextContinuationToken;

        } while (response.IsTruncated ?? false);
        
    }

    public async Task SaveMetadataAsync(string fileName, string metadata)
    {
        await redisRepository.SetValue(fileName, metadata);
        if (await redisRepository.GetSize() >= 2)
        {
            await UploadAllFilesToPermAsync();
            await redisRepository.ClearDatabaseAsync();
        }
    }

    /// <summary>
    /// Возвращает набор метаданных в формате Json.
    /// Возвращает пустую строку если тип файла не поддерживается (=файл без метаданных).
    /// Выбрасывает исключение MetadataUploadError, если произошла ошибка при получении метаданных.
    /// </summary>
    public string GetMetadataJson(IFormFile file)
    {
        var tags = new Dictionary<string, string>();
        try
        {
            using var tagFile = File.Create(new StreamFileAbstraction(file));
            var tag = tagFile.Tag;
            foreach (var property in tag.GetType().GetProperties())
            {
                var value = property.GetValue(tag);
                switch (value)
                {
                    case null:
                    case TagLib.Tag:
                    case double.NaN:
                    case IEnumerable<TagLib.Tag>:
                        continue;
                    case string[] array:
                    {
                        if (array.Length > 0)
                            tags[property.Name] = string.Join(',', array);
                        break;
                    }
                    default:
                        tags[property.Name] = value.ToString()!;
                        break;
                }
            }
        }
        catch (TagLib.UnsupportedFormatException)
        {
            return string.Empty;
        }
        catch(Exception ex)
        {
            throw new MetadataUploadError(file.FileName, ex.Message);
        }


        return JsonConvert.SerializeObject(tags, Formatting.Indented);;
    }

    public async Task<(Stream stream, string contentType, string fileName)> GetFileAsync(string fileName)
    {
        try
        {
            var s3Object = await s3Client.GetObjectAsync(_permBucketName, fileName);
            return (s3Object.ResponseStream, s3Object.Headers.ContentType, fileName.Split(':')[1]);
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode != HttpStatusCode.NotFound) throw;
            var s3Object = await s3Client.GetObjectAsync(_tempBucketName, fileName);
            return (s3Object.ResponseStream, s3Object.Headers.ContentType, fileName.Split(':')[1]);
        }
    }
}