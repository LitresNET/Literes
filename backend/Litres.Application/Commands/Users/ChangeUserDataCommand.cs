using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users;

public record ChangeUserDataCommand(string Name, string AvatarUrl) : ICommand<UserSettingsDto>
{
    //TODO: Перенести UserId в декоратор и заполнять автоматически
    public long UserId { get; set; }
}