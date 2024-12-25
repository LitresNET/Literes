using Litres.Domain.Abstractions.Commands;
using Microsoft.AspNetCore.Http;

namespace Litres.Application.Commands.Files;

public record UploadFileCommand(IFormFile File, long ChatId, long UserId) : ICommand<string>
{
    public IFormFile File { get; } = File;
    public long ChatId { get; } = ChatId;
    public long UserId { get; } = UserId;
}   