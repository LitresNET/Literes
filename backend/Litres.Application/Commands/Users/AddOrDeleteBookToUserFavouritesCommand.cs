using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users;

public record AddOrDeleteBookToUserFavouritesCommand(long BookId) : ICommand
{
    public long UserId { get; set; }
}