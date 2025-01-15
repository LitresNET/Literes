using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;

namespace Litres.Domain.Abstractions.Services;

public interface IFileService
{
    Task<string> UploadFileToTemp(IFormFile file, long userId);
    Task<string> UploadFileToPerm(string fileName);
    Task UploadAllFilesToPerm();
    Task SaveMetadata(string fileName, string metadata);
    string GetMetadata(IFormFile file);
    Task<Stream> GetFile(string fileName);
}