using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;

namespace Litres.Application.Commands.Files.Handlers;

public class UploadFileToTempCommandHandler(
    IFileService service
    ) : ICommandHandler<UploadFileToTempCommand, string>
{

    
    public async Task<string> HandleAsync(UploadFileToTempCommand toTempCommand)
    {
        return await service.UploadFileToTemp(toTempCommand.File, toTempCommand.UserId);
    }
}

