using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Commands.Files;

public record UploadFileCommand(IFormFile File) : ICommand
{
}   