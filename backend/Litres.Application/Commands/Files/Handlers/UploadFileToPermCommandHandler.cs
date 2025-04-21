using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;

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