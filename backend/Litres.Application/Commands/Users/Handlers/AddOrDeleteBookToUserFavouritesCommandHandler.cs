using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users.Handlers;

public class AddOrDeleteBookToUserFavouritesCommandHandler(IUserRepository userRepository, 
    IBookRepository bookRepository) : ICommandHandler<AddOrDeleteBookToUserFavouritesCommand>
{
    public async Task HandleAsync(AddOrDeleteBookToUserFavouritesCommand command)
    {
        var dbUser = await userRepository.GetByIdAsync(command.UserId);
        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == command.BookId);
        if (book is not null)
            dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        else
        {
            book = await bookRepository.GetByIdAsync(command.BookId);
            dbUser.Favourites.Add(book);
        }
        await userRepository.SaveChangesAsync();
    }
}