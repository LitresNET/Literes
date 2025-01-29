using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Files;

public record UploadFileToPermCommand(string FileName) : ICommand<string>;