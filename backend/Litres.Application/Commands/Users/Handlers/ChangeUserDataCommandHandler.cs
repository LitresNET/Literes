using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Users.Handlers;

public class ChangeUserDataCommandHandler(IUserRepository userRepository, IMapper mapper) 
    : ICommandHandler<ChangeUserDataCommand, UserSettingsDto>
{
    public async Task<UserSettingsDto> HandleAsync(ChangeUserDataCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);
        
        dbUser.Name = command.Name;
        dbUser.AvatarUrl = command.AvatarUrl;
        
        await userRepository.SaveChangesAsync();
        return mapper.Map<UserSettingsDto>(dbUser);
    }
}