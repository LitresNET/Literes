using System.Text.Json.Serialization;
using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Commands.Files;

public record UploadFileToTempCommand(IFormFile File) : ICommand<string>
{
    public IFormFile File { get; } = File;
    [JsonIgnore]
    public long UserId { get; set; }
}   