using System.Security.Claims;
using Litres.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Litres.Data.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);
}