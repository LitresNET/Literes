using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Microsoft.Extensions.Configuration;

namespace Litres.Application.Commands.Files.Handlers;

//TODO: можно удалить, заменено service
public class UploadFileToPermCommandHandler(
    IFileService fileService
) : ICommandHandler<UploadFileToPermCommand, string>
{

    public async Task<string> HandleAsync(UploadFileToPermCommand command)
    {
        return await fileService.UploadFileToPermAsync(command.FileName);
    }
}