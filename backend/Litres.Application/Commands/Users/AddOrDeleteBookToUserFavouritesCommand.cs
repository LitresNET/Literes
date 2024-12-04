using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users;

public record AddOrDeleteBookToUserFavouritesCommand(long BookId) : ICommand
{
    //TODO: Перенести UserId в декоратор и заполнять автоматически
    public long UserId { get; set; }
}