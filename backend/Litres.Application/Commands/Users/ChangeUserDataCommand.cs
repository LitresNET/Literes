using System.Text.Json.Serialization;
using Litres.Application.Dto.Requests;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users;

public record ChangeUserDataCommand(string Name, string AvatarUrl) : ICommand<UserSettingsDto>
{
    [JsonIgnore] //чтобы в сваггере не отображалось это
    public long UserId { get; set; }
}