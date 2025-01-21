using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;

namespace Litres.Domain.Abstractions.Services;

public interface IFileService
{
    Task<string> UploadFileToTempAsync(IFormFile file, long userId);
    Task<string?> UploadFileToPermAsync(string fileName);
    Task UploadAllFilesToPermAsync();
    Task SaveMetadataAsync(string fileName, string metadata);
    string GetMetadataJson(IFormFile file);
    Task<(Stream stream, string contentType, string fileName)> GetFileAsync(string fileName);
}